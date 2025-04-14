"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { ArrowLeft, Calendar, CreditCard, Download, Filter, Search, SortAsc, SortDesc } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Badge } from "@/components/ui/badge"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import "./PStyle.css"

import { cards } from '@/mocks/clientMocks/clientCards'

// Datos de ejemplo para tarjetas de crédito
const creditCards = [
    {
        id: "1",
        name: "Tarjeta de Recompensas Platinum",
        number: "**** **** **** 5678",
        expiryDate: "05/25",
        availableCredit: 8500,
        totalLimit: 10000,
        balance: 1500,
        minPayment: 35,
        dueDate: "2023-05-18",
        apr: 18.99,
        issuer: "Visa",
    },
    {
        id: "2",
        name: "Mastercard de Reembolso",
        number: "**** **** **** 1234",
        expiryDate: "09/24",
        availableCredit: 3200,
        totalLimit: 5000,
        balance: 1800,
        minPayment: 45,
        dueDate: "2023-05-22",
        apr: 15.99,
        issuer: "Mastercard",
    },
]

// Datos de ejemplo para compras
const allPurchases = [
    {
        id: "1",
        cardId: "1",
        date: "2023-04-01",
        merchant: "Amazon",
        category: "Compras",
        amount: 125.99,
        status: "posted",
    },
    {
        id: "2",
        cardId: "1",
        date: "2023-04-03",
        merchant: "Starbucks",
        category: "Restaurantes",
        amount: 7.85,
        status: "posted",
    },
    {
        id: "3",
        cardId: "1",
        date: "2023-04-05",
        merchant: "Uber",
        category: "Transporte",
        amount: 24.5,
        status: "posted",
    },
    {
        id: "4",
        cardId: "1",
        date: "2023-04-08",
        merchant: "Target",
        category: "Compras",
        amount: 87.32,
        status: "posted",
    },
    {
        id: "5",
        cardId: "1",
        date: "2023-04-10",
        merchant: "Netflix",
        category: "Entretenimiento",
        amount: 14.99,
        status: "posted",
    },
    {
        id: "6",
        cardId: "2",
        date: "2023-04-02",
        merchant: "Whole Foods",
        category: "Supermercado",
        amount: 89.75,
        status: "posted",
    },
    {
        id: "7",
        cardId: "2",
        date: "2023-04-04",
        merchant: "Shell",
        category: "Gasolina",
        amount: 45.23,
        status: "posted",
    },
    {
        id: "8",
        cardId: "2",
        date: "2023-04-07",
        merchant: "Apple",
        category: "Tecnología",
        amount: 999.99,
        status: "pending",
    },
    {
        id: "9",
        cardId: "2",
        date: "2023-04-09",
        merchant: "Chipotle",
        category: "Restaurantes",
        amount: 15.47,
        status: "posted",
    },
    {
        id: "10",
        cardId: "2",
        date: "2023-04-12",
        merchant: "Spotify",
        category: "Entretenimiento",
        amount: 9.99,
        status: "posted",
    },
]

// Categorías para filtrado
const categories = [
    "Todas",
    "Compras",
    "Restaurantes",
    "Transporte",
    "Entretenimiento",
    "Supermercado",
    "Gasolina",
    "Tecnología",
]

function CardPurchases() {

    const navigate = useNavigate()

    // Estado para filtros
    const [selectedCardId, setSelectedCardId] = useState("")
    const [startDate, setStartDate] = useState("")
    const [endDate, setEndDate] = useState("")
    const [searchTerm, setSearchTerm] = useState("")
    const [categoryFilter, setCategoryFilter] = useState("Todas")
    const [sortField, setSortField] = useState("date")
    const [sortDirection, setSortDirection] = useState("desc")


    // Filtrar y ordenar compras
    const filteredPurchases = allPurchases.filter((purchase) => {
        // Filtrar por tarjeta
        if (selectedCardId && purchase.cardId !== selectedCardId) return false

        // Filtrar por rango de fechas
        if (startDate && new Date(purchase.date) < new Date(startDate)) return false
        if (endDate && new Date(purchase.date) > new Date(endDate)) return false

        // Filtrar por término de búsqueda (comerciante o categoría)
        if (
            searchTerm &&
            !purchase.merchant.toLowerCase().includes(searchTerm.toLowerCase()) &&
            !purchase.category.toLowerCase().includes(searchTerm.toLowerCase())
        )
            return false

        // Filtrar por categoría
        if (categoryFilter !== "Todas" && purchase.category !== categoryFilter) return false

        return true
    })

    // Ordenar compras
    const sortedPurchases = [...filteredPurchases].sort((a, b) => {
        if (sortField === "date") {
            return sortDirection === "asc" ? new Date(a.date) - new Date(b.date) : new Date(b.date) - new Date(a.date)
        } else if (sortField === "amount") {
            return sortDirection === "asc" ? a.amount - b.amount : b.amount - a.amount
        } else if (sortField === "merchant") {
            return sortDirection === "asc" ? a.merchant.localeCompare(b.merchant) : b.merchant.localeCompare(a.merchant)
        }
        return 0
    })

    // Manejar cambio de ordenamiento
    const handleSort = (field) => {
        if (sortField === field) {
            setSortDirection(sortDirection === "asc" ? "desc" : "asc")
        } else {
            setSortField(field)
            setSortDirection("asc")
        }
    }

    // Formatear fecha para mostrar
    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString()
    }

    // Calcular monto total
    const totalAmount = sortedPurchases.reduce((sum, purchase) => sum + purchase.amount, 0)

    return (
        <div className="purchases-container">
            <div className="purchases-content">
                {/* Botón de regreso */}
                <div className="back-link-container">
                    <Button variant="ghost" className="back-link" onClick={() => navigate("/client_dashboard")}>
                        <ArrowLeft className="back-icon" />
                        <span>Volver al Panel</span>
                    </Button>
                </div>

                <div className="purchases-header">
                    <h1 className="page-title">Compras con Tarjeta</h1>
                    <p className="page-description">Ver y filtrar sus compras con tarjeta de crédito por rango de fechas</p>
                </div>

                <div className="purchases-card">
                    {/* Sección de Filtros */}
                    <div className="filters-section">
                        <div className="filters-header">
                            <h2 className="filters-title">
                                <Filter className="filters-icon" />
                                Filtrar Compras
                            </h2>
                        </div>

                        <div className="filters-grid">


                            {/* Rango de Fechas */}
                            <div className="filter-group">
                                <Label htmlFor="startDate" className="filter-label">
                                    <Calendar className="filter-label-icon" />
                                    Fecha Inicial
                                </Label>
                                <Input
                                    id="startDate"
                                    type="date"
                                    value={startDate}
                                    onChange={(e) => setStartDate(e.target.value)}
                                    className="filter-input"
                                />
                            </div>

                            <div className="filter-group">
                                <Label htmlFor="endDate" className="filter-label">
                                    <Calendar className="filter-label-icon" />
                                    Fecha Final
                                </Label>
                                <Input
                                    id="endDate"
                                    type="date"
                                    value={endDate}
                                    onChange={(e) => setEndDate(e.target.value)}
                                    className="filter-input"
                                />
                            </div>
                        </div>
                    </div>

                    {/* Sección de Resultados */}
                    <div className="results-section">
                        <div className="results-header">
                        </div>

                        {sortedPurchases.length > 0 ? (
                            <div className="table-container">
                                <Table>
                                    <TableHeader>
                                        <TableRow>
                                            <TableHead className="date-column">
                                                <button className="sort-button" onClick={() => handleSort("date")}>
                                                    Fecha
                                                    {sortField === "date" && (
                                                        <span className="sort-icon">
                                                            {sortDirection === "asc" ? (
                                                                <SortAsc className="sort-icon-svg" />
                                                            ) : (
                                                                <SortDesc className="sort-icon-svg" />
                                                            )}
                                                        </span>
                                                    )}
                                                </button>
                                            </TableHead>
                                            <TableHead className="merchant-column">
                                                <button className="sort-button" onClick={() => handleSort("merchant")}>
                                                    Comerciante
                                                    {sortField === "merchant" && (
                                                        <span className="sort-icon">
                                                            {sortDirection === "asc" ? (
                                                                <SortAsc className="sort-icon-svg" />
                                                            ) : (
                                                                <SortDesc className="sort-icon-svg" />
                                                            )}
                                                        </span>
                                                    )}
                                                </button>
                                            </TableHead>
                                            <TableHead className="category-column">Categoría</TableHead>
                                            <TableHead className="card-column">Tarjeta</TableHead>
                                            <TableHead className="amount-column">
                                                <button className="sort-button" onClick={() => handleSort("amount")}>
                                                    Monto
                                                    {sortField === "amount" && (
                                                        <span className="sort-icon">
                                                            {sortDirection === "asc" ? (
                                                                <SortAsc className="sort-icon-svg" />
                                                            ) : (
                                                                <SortDesc className="sort-icon-svg" />
                                                            )}
                                                        </span>
                                                    )}
                                                </button>
                                            </TableHead>
                                            <TableHead className="status-column">Estado</TableHead>
                                        </TableRow>
                                    </TableHeader>
                                    <TableBody>
                                        {sortedPurchases.map((purchase) => {
                                            const card = creditCards.find((c) => c.id === purchase.cardId)
                                            return (
                                                <TableRow key={purchase.id}>
                                                    <TableCell className="date-cell">{formatDate(purchase.date)}</TableCell>
                                                    <TableCell className="merchant-cell">{purchase.merchant}</TableCell>
                                                    <TableCell className="category-cell">
                                                        <Badge variant="outline" className="category-badge">
                                                            {purchase.category}
                                                        </Badge>
                                                    </TableCell>
                                                    <TableCell className="card-cell">{card ? card.name.split(" ")[0] : ""}</TableCell>
                                                    <TableCell className="amount-cell">${purchase.amount.toFixed(2)}</TableCell>
                                                    <TableCell className="status-cell">
                                                        <Badge
                                                            variant="outline"
                                                            className={`status-badge ${purchase.status === "pending" ? "status-pending" : "status-posted"
                                                                }`}
                                                        >
                                                            {purchase.status === "pending" ? "Pendiente" : "Registrado"}
                                                        </Badge>
                                                    </TableCell>
                                                </TableRow>
                                            )
                                        })}
                                    </TableBody>
                                </Table>
                            </div>
                        ) : (
                            <div className="no-results">
                                <div className="no-results-icon-container">
                                    <CreditCard className="no-results-icon" />
                                </div>
                                <h3 className="no-results-title">No Se Encontraron Compras</h3>
                                <p className="no-results-message">
                                    Intente ajustar sus filtros o seleccionar un rango de fechas diferente para ver compras.
                                </p>
                            </div>
                        )}
                    </div>
                </div>
            </div>
            <Toaster position="top-center" />
        </div>
    )
}

export default CardPurchases