"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { ArrowLeft, ArrowUpRight, ArrowDownRight, CreditCard, Download, Filter, Search } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import "./TStyle.css"

import { cards } from "@/mocks/clientMocks/clientCards"
import { sourceAccounts } from "@/mocks/clientMocks/clientAccounts"

// Datos de ejemplo para transacciones
const transactions = [
  {
    id: "1",
    date: "2023-04-01",
    description: "Depósito de Salario",
    type: "deposit",
    amount: 2500.0,
    balance: 3200.5,
  },
  {
    id: "2",
    date: "2023-04-02",
    description: "Supermercado",
    type: "withdrawal",
    amount: 85.75,
    balance: 3114.75,
  },
  {
    id: "3",
    date: "2023-04-03",
    description: "Compra en Línea",
    type: "withdrawal",
    amount: 129.99,
    balance: 2984.76,
  },
  {
    id: "4",
    date: "2023-04-05",
    description: "Restaurante",
    type: "withdrawal",
    amount: 45.8,
    balance: 2938.96,
  },
  {
    id: "5",
    date: "2023-04-10",
    description: "Retiro en Cajero",
    type: "withdrawal",
    amount: 200.0,
    balance: 2738.96,
  },
  {
    id: "6",
    date: "2023-04-15",
    description: "Pago de Intereses",
    type: "deposit",
    amount: 12.5,
    balance: 2751.46,
  },
  {
    id: "7",
    date: "2023-04-20",
    description: "Factura de Servicios",
    type: "withdrawal",
    amount: 95.4,
    balance: 2656.06,
  },
]

function AccountTransactions() {

  const navigate = useNavigate()
  const [searchTerm, setSearchTerm] = useState("")
  const [filterType, setFilterType] = useState("all")

  // Filtrar transacciones basadas en término de búsqueda y tipo de filtro
  const filteredTransactions = transactions.filter((transaction) => {
    const matchesSearch = transaction.description.toLowerCase().includes(searchTerm.toLowerCase())
    const matchesType = filterType === "all" || transaction.type === filterType
    return matchesSearch && matchesType
  })

  // Cambio de moneda
  const getCurrencySymbol = (currencyId) => {
    const currencySymbols = {
      1: '$', // Dólar
      2: '€', // Euro
      3: '₡'  // Colón costarricense
    };
    return currencySymbols[currencyId] || '$';
  };

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
                        <TableHead className="table-header-cell text-right">Monto</TableHead>
                        <TableHead className="table-header-cell text-right">Saldo</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {filteredTransactions.length > 0 ? (
                        filteredTransactions.map((transaction) => (
                          <TableRow key={transaction.id} className="table-row">
                            <TableCell className="table-cell date-cell">
                              {new Date(transaction.date).toLocaleDateString()}
                            </TableCell>
                            <TableCell className="table-cell description-cell">{transaction.description}</TableCell>
                            <TableCell className="table-cell type-cell">
                              {transaction.type === "deposit" ? (
                                <span className="deposit-badge">
                                  <ArrowUpRight className="deposit-icon" />
                                  Depósito
                                </span>
                              ) : (
                                <span className="withdrawal-badge">
                                  <ArrowDownRight className="withdrawal-icon" />
                                  Retiro
                                </span>
                              )}
                            </TableCell>
                            <TableCell
                              className={`table-cell amount-cell ${transaction.type === "deposit" ? "deposit-amount" : "withdrawal-amount"
                                }`}
                            >
                              {transaction.type === "deposit" ? "+" : "-"}${transaction.amount.toFixed(2)}
                            </TableCell>
                            <TableCell className="table-cell balance-cell">${transaction.balance.toFixed(2)}</TableCell>
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
                .filter(card => card.type === 1)  // Filtra tarjetas de débito
                .map((card) => {
                  const account = sourceAccounts.find(acc => acc.id === card.account_id);
                  const currencyId = account ? account.currency_id : 1;

                  return (
                    <div key={card.card_num} className="debit-card">
                      <div className="card-header">
                        <div className="card-header-content">
                          <div className="card-info">
                            <h3 className="card-type">
                              {card.type === 1 ? "Débito" : "Crédito"}
                            </h3>
                            <p className="card-number">{card.card_num}</p>
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
                              {getCurrencySymbol(currencyId)} {currencyId === 1 ? 'USD:  ' : currencyId === 2 ? 'EUR:  ' : 'CRC:  '}
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