"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { ArrowLeft, ArrowUpRight, ArrowDownRight, CreditCard } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { useAuth } from "@/context/AuthContext"
import "./transactionsStyle.css"

function AccountTransactions() {
  const navigate = useNavigate()
  const { user } = useAuth()
  const [searchTerm, setSearchTerm] = useState("")
  const [filterType, setFilterType] = useState("all")
  const [transactions, setTransactions] = useState([])
  const [cards, setCards] = useState([])
  const [accounts, setAccounts] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [selectedAccount, setSelectedAccount] = useState(null)

  // Función para obtener el tipo de movimiento como texto
  const getMovementType = (type) => {
    const types = {
      1: "Transferencia",
      2: "Pago Tarjeta Crédito",
      3: "Pago Préstamo", 
      4: "Retiro ATM",
      5: "Ajuste Límite Débito"
    };
    return types[type] || "Movimiento";
  };

  // Obtener cuentas y tarjetas
  useEffect(() => {
    if (!user?.id) return;

    const fetchInitialData = async () => {
      try {
        setLoading(true);
        
        // Obtener tarjetas
        const cardsResponse = await fetch(
          `http://192.168.100.59:5055/services/client/${user.id}/cards`
        );
        const cardsData = await cardsResponse.json();
        setCards(cardsData);
        
        // Obtener cuentas
        const accountsResponse = await fetch(
          `http://192.168.100.59:5055/services/client/${user.id}/accounts`
        );
        const accountsData = await accountsResponse.json();
        setAccounts(accountsData);
        
        // Seleccionar primera cuenta por defecto
        if (accountsData.length > 0) {
          setSelectedAccount(accountsData[0].id);
        }
      } catch (err) {
        console.error("Error fetching data:", err);
        setError("No se pudieron cargar los datos iniciales");
      } finally {
        setLoading(false);
      }
    };

    fetchInitialData();
  }, [user?.id]);

  // Obtener movimientos cuando cambia la cuenta seleccionada
  useEffect(() => {
    if (!selectedAccount || !user?.id) return;

    const fetchMovements = async () => {
      try {
        setLoading(true);
        const response = await fetch(
          `http://192.168.100.59:5055/services/client/${user.id}/${selectedAccount}/movements`
        );
        const movementsData = await response.json();

        const normalized = movementsData.map((tx, index) => ({
          ...tx,
          id: tx.id ?? `${index}-${Date.now()}`,
          amount: tx.amount ?? tx.total_transfer ?? 0,
          type: tx.type,
          typeText: getMovementType(tx.type),
          isDeposit: [1, 5].includes(tx.type),
        }));

        setTransactions(normalized);
      } catch (err) {
        console.error("Error fetching movements:", err);
        setError("No se pudieron cargar los movimientos");
      } finally {
        setLoading(false);
      }
    };

    fetchMovements();
  }, [selectedAccount, user?.id]);

  // Filtrar transacciones
  const filteredTransactions = transactions.filter((transaction) => {
    const matchesSearch = transaction.description.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesType = filterType === "all" || 
                       (filterType === "deposit" && transaction.isDeposit) ||
                       (filterType === "withdrawal" && !transaction.isDeposit);
    return matchesSearch && matchesType;
  });

  // Manejo de simbolo de moneda
  const getCurrencySymbol = () => {
    if (!selectedAccount) return '$';
    const account = accounts.find(acc => acc.id === selectedAccount);
    const currencySymbols = { 1: '$', 2: '€', 3: '₡' };
    return currencySymbols[account?.currency_id || 1] || '$';
  };

  if (loading) {
    return (
      <div className="transactions-container">
        <div className="loading-container">
          <p>Cargando datos...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="transactions-container">
        <div className="error-container">
          <p>{error}</p>
          <Button onClick={() => window.location.reload()}>Reintentar</Button>
        </div>
      </div>
    );
  }

  return (
    <div className="transactions-container">
      <div className="transactions-content">

        {/* Botón de regreso */}
        <div className="back-link-container">
          <Button variant="ghost" className="back-link-client" onClick={() => navigate("/client_dashboard")}>
            <ArrowLeft className="back-icon" />
            <span>Volver al Panel</span>
          </Button>
        </div>

        <div className="page-header">
          <h1 className="page-title">Transacciones de Cuenta</h1>
          <p className="page-description">Ver su historial de transacciones y tarjetas de débito asociadas</p>
        </div>

        <Tabs defaultValue="transactions" className="tabs-container">

          <TabsList className="tabs-list">
            <TabsTrigger value="transactions" className="tab-button">
              Transacciones
            </TabsTrigger>
            <TabsTrigger value="cards" className="tab-button">
              Tarjetas de Débito
            </TabsTrigger>
          </TabsList>

          <TabsContent value="transactions" className="space-y-6">

            {/* Tabla de transacciones */}
            <Card className="transactions-table-card">
              <CardContent className="p-0">
                <div className="table-container">
                  <Table className="transactions-table">

                    <TableHeader className="table-header">
                      <TableRow className="table-row">
                        <TableHead className="table-header-cell">Fecha</TableHead>
                        <TableHead className="table-header-cell">Descripción</TableHead>
                        <TableHead className="table-header-cell">Tipo</TableHead>
                        <TableHead className="table-header-cell">Monto</TableHead>
                      </TableRow>
                    </TableHeader>
                      <TableBody>
                        {filteredTransactions.length > 0 ? (
                          filteredTransactions.map((transaction) => (
                            <TableRow key={transaction.id} className="table-row">
                              <TableCell className="table-cell date-cell">
                                {new Date(transaction.date).toLocaleDateString()}
                              </TableCell>

                              <TableCell className="table-cell description-cell">
                                {transaction.description}
                              </TableCell>

                              <TableCell className="table-cell type-cell">
                                <span className={`type-badge ${
                                  transaction.isDeposit ? "deposit-badge" : "withdrawal-badge"
                                }`}>
                                  {transaction.typeText}
                                </span>
                              </TableCell>

                              <TableCell className={`table-cell amount-cell ${
                                transaction.isDeposit ? "deposit-amount" : "withdrawal-amount"
                              }`}>
                                {getCurrencySymbol()}
                                {transaction.isDeposit ? '+' : '-'}
                                {Math.abs(transaction.amount).toFixed(2)}
                              </TableCell>
                            </TableRow>
                          ))
                        ) : (
                          <TableRow>
                            <TableCell colSpan={5} className="empty-state">
                              No se encontraron transacciones que coincidan con sus criterios
                            </TableCell>
                          </TableRow>
                        )}
                    </TableBody>
                  </Table>
                </div>
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="cards" className="space-y-6">
            <div className="cards-grid-transactions">
              {cards
                .filter(card => card.type === 2)  // Filtra tarjetas de débito
                .map((card) => {
                  const account = sourceAccounts.find(acc => acc.id === card.account_id)
                  return (
                    <div key={card.card_num} className="debit-card">
                      <div className="card-header">
                        <div className="card-header-content">
                          <div className="card-info">
                            <h3 className="card-type">
                              {card.type === 2 ? "Débito" : "Crédito"}
                            </h3>
                            <p className="card-number">Número : {card.card_num}</p>
                          </div>
                          <CreditCard className="card-icon" />
                        </div>
                      </div>
                      <div className="card-details">
                        <div className="card-details-grid">
                          <div className="card-detail-item">
                            <p className="detail-label">Cuenta asociada</p>
                            <p className="detail-value">
                              {account ? account.description : card.account_id}
                            </p>
                          </div>
                          <div className="card-detail-item">
                            <p className="detail-label">Saldo disponible</p>
                            <span className="detail-value">
                              {getCurrencySymbol(transactions.currency_id)}
                              {card.balance.toFixed(2)}
                            </span>
                          </div>
                        </div>
                      </div>
                    </div>
                  );
                })}
            </div>
          </TabsContent>
        </Tabs>

      </div>
    </div>
  )
}

export default AccountTransactions