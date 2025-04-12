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
import "./transactionsStyle.css"
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

// Datos de ejemplo para tarjetas de débito
const debitCards = [
  {
    id: "1",
    cardNumber: "**** **** **** 4567",
    expiryDate: "05/25",
    cardType: "Visa Débito",
    status: "active",
  },
  {
    id: "2",
    cardNumber: "**** **** **** 8901",
    expiryDate: "09/24",
    cardType: "Mastercard Débito",
    status: "active",
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
            {/* Filtros y búsqueda */}
            <div className="filters-row">
              <div className="filter-group">
                <Filter className="filter-icon" />
                <Select value={filterType} onValueChange={setFilterType}>
                  <SelectTrigger className="filter-select">
                    <SelectValue placeholder="Filtrar por tipo" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="all">Todas las Transacciones</SelectItem>
                    <SelectItem value="deposit">Solo Depósitos</SelectItem>
                    <SelectItem value="withdrawal">Solo Retiros</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="search-group">
                <div className="search-input-container">
                  <Search className="search-icon" />
                  <Input
                    type="search"
                    placeholder="Buscar transacciones..."
                    className="search-input"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                  />
                </div>
                <button className="download-button">
                  <Download className="download-icon" />
                  <span className="sr-only">Descargar transacciones</span>
                </button>
              </div>
            </div>

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
                              className={`table-cell amount-cell ${
                                transaction.type === "deposit" ? "deposit-amount" : "withdrawal-amount"
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
              {debitCards.map((card) => (
                <div key={card.id} className="debit-card">
                  <div className="card-header">
                    <div className="card-header-content">
                      <div className="card-info">
                        <h3 className="card-type">{card.cardType}</h3>
                        <p className="card-number">{card.cardNumber}</p>
                      </div>
                      <CreditCard className="card-icon" />
                    </div>
                  </div>
                  <div className="card-details">
                    <div className="card-details-grid">
                      <div className="card-detail-item">
                        <p className="detail-label">Fecha de Vencimiento</p>
                        <p className="detail-value">{card.expiryDate}</p>
                      </div>
                      <div className="card-detail-item">
                        <p className="detail-label">Estado</p>
                        <span className="card-status">Activa</span>
                      </div>
                    </div>
                    <div className="card-actions">
                      <button className="card-action-button">Ver Detalles</button>
                      <button className="card-action-button block-button">Bloquear Tarjeta</button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </TabsContent>

        </Tabs>
      </div>
    </div>
  )
}

export default AccountTransactions