import { useState, useEffect } from "react"
import { ArrowLeft, Plus, Pencil, Trash2, Search, X, DollarSign } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import "./gestion_tarjetas.css"

export default function CardManagement({ onBack }) {
    
    // Ejemplos de cuentas de clientes (info necesaria paara crear una tarjeta)
    const [accounts, setAccounts] = useState([
        {
        account_id: "ACC-10001",
        client_id: "123456789",
        },
        {
        account_id: "ACC-10002",
        client_id: "CORP123456",
        },
        {
        account_id: "ACC-10003",
        client_id: "987654321",
        },
        {
        account_id: "ACC-10004",
        client_id: "123456789",
        },
    ])

    // Ejemplo de como debería ir la información de la tarjeta (así lo manda el backend excepto el client_id)
    const [cards, setCards] = useState([
        {
        card_number: 4532123456781234,
        type: 1, // crédito
        cvc: 123,
        balance: 5000.75,
        account_id: "ACC-10001",
        rem_state: 0,
        client_id: "123456789", // Agregado para filtrar
        },
        {
        card_number: 5412345678901234,
        type: 2, // débito
        cvc: 456,
        balance: 2500.5,
        account_id: "ACC-10002",
        rem_state: 0,
        client_id: "CORP123456", // Agregado para filtrar
        },
        {
        card_number: 4111222233334444,
        type: 1, // crédito
        cvc: 789,
        balance: 10000.0,
        account_id: "ACC-10003",
        rem_state: 0,
        client_id: "987654321", // Agregado para filtrar
        },
        {
        card_number: 5555666677778888,
        type: 2, // débito
        cvc: 321,
        balance: 7500.25,
        account_id: "ACC-10004",
        rem_state: 0,
        client_id: "123456789", // Agregado para filtrar
        },
    ])

    // Estados para filtrar
    const [clientIdFilter, setClientIdFilter] = useState("")
    const [filteredCards, setFilteredCards] = useState(cards) // al modificar el la barra de filtrar se colocan las tarjetas filtradas aqui

    // Actualiza las tarjetas filtradas cuando la barra de filtro cambia
    // Función donde se debería hacer consultas
    useEffect(() => {
        if (clientIdFilter.trim() === "") {
            setFilteredCards(cards)
        } else {
            const filtered = cards.filter((card) => card.client_id.toLowerCase().includes(clientIdFilter.toLowerCase()))
            setFilteredCards(filtered)
        }
    }, [clientIdFilter, cards])

    // Estados para cuando la tarjeta se edita o se agrega una nueva
    const [editingCard, setEditingCard] = useState(null)
    const [isCardDialogOpen, setIsCardDialogOpen] = useState(false)
    const [newCard, setNewCard] = useState({
        type: 1,
        balance: "",
        account_id: "",
        rem_state: 0,
    })

    // Función que genera un nuevo numero de una tarjeta (16 digitos) para simular backend
    const generateCardNumber = () => {

        // Empieza con 4 para Visa o 5 para MasterCard
        const prefix = Math.random() < 0.5 ? "4" : "5"
        let cardNumber = prefix

        // Genera otros 15 numeros random
        for (let i = 0; i < 15; i++) {
            cardNumber += Math.floor(Math.random() * 10)
        }

        return Number(cardNumber)
    }

    // Se generan un numero de 3 digitos para el CVC
    const generateCVC = () => {
        return Math.floor(Math.random() * 900) + 100 // 100-999
    }

    // Función para obtener el ID del cliente desde los ejemplos de cuenta
    // Enlace entre cuentas y ID, necesario para crear tarjetas (esto lo hace el backend)
    const getClientIdFromAccount = (accountId) => {
        const account = accounts.find((acc) => acc.account_id === accountId)
        return account ? account.client_id : ""
    }

    // Función para agregar una nueva tarjeta
    const handleAddCard = () => {
        const card_number = generateCardNumber()
        const cvc = generateCVC()
        const client_id = getClientIdFromAccount(newCard.account_id)

        // Conversión del balance en string a numero
        const balance = newCard.balance === "" ? 0 : Number.parseFloat(newCard.balance)
        
        // Se hace una copia de las tarjetas ya existentes y se concatena con la nueva
        setCards([...cards,{...newCard,card_number,cvc,balance,client_id}])

        // Se resetea el "molde" de una nueva tarjeta
        setNewCard({
            type: 1,
            balance: "",
            account_id: "",
            rem_state: 0,
        })

        setIsCardDialogOpen(false)
    }

    // Función para editar una tarjeta
    const handleEditCard = () => {
        // Se convierte el balance de la tarjeta editada a numero
        const balance = editingCard.balance === "" ? 0 : Number.parseFloat(editingCard.balance)
        const updatedCard = { ...editingCard, balance }

        setCards(cards.map((card) => (card.card_number === updatedCard.card_number ? updatedCard : card)))
        setEditingCard(null)
        setIsCardDialogOpen(false)
    }

    // Función para eliminar una tarjeta
    const handleDeleteCard = (card_number) => {
        setCards(cards.filter((card) => card.card_number !== card_number))
    }

    // Función para abrir el formulario o caja de dialogo de una nueva tarjeta
    const openAddCardDialog = () => {
        setEditingCard(null)
        setNewCard({
            type: 1,
            balance: "",
            account_id: "",
            rem_state: 0,
        })
        setIsCardDialogOpen(true)
    }

    // Función para abrir el formulario o caja de dialogo para editar tarjeta
    const openEditCardDialog = (card) => {
        setEditingCard({ ...card })
        setIsCardDialogOpen(true)
    }

    // Función para limpiar la barra de filtro
    const clearFilter = () => {
        setClientIdFilter("")
    }

    // Función para enmascarar los numero de la tarjeta como seguridad
    const maskCardNumber = (number) => {
        const numStr = number.toString()
        return `${numStr.slice(0, 4)} **** **** ${numStr.slice(12, 16)}`
    }

    // Función para obtener el label del tipo de la tarjeta (texto que se observa en la tabla)
    const getCardTypeLabel = (type) => {
        return type === 1 ? "Crédito" : "Débito"
    }

    return (
        <div className="card-management-container">
            <div className="header-container">
                <Button variant="ghost" className="mr-4" onClick={onBack}>
                    <ArrowLeft className="mr-2 h-4 w-4" /> Volver al panel de control
                </Button>
                <h1 className="header-title">Gestión de Tarjetas</h1>
            </div>

            <div className="title-container">
                <p className="text-muted-foreground">Administración de tarjetas bancarias y su información.</p>
                <Button onClick={openAddCardDialog}>
                    <Plus className="mr-2 h-4 w-4" /> Crear una tarjeta
                </Button>
            </div>

            {/* Barra de búsqueda por ID de cliente (filtro) */}
            <div className="search-container">
                <div className="search-input-container">
                    <Search className="search-icon" />
                    <Input
                        placeholder="Buscar por identificación de cliente..."
                        value={clientIdFilter}
                        onChange={(e) => setClientIdFilter(e.target.value)}
                        className="pl-10 pr-10"
                    />
                    {clientIdFilter && (
                        <Button
                            variant="ghost"
                            size="icon"
                            className="clear-filter-button"
                            onClick={clearFilter}
                        >
                            <X className="h-4 w-4" />
                            <span className="sr-only">Limpiar filtro</span>
                        </Button>
                    )}
                </div>
                {filteredCards.length === 0 && (
                <p className="no-results-text">Sin resultados</p>
                )}
            </div>

            <Card>
                <CardContent className="p-0">
                    <Table>
                        <TableHeader>
                        <TableRow>
                            <TableHead>Número de tarjeta</TableHead>
                            <TableHead>Tipo</TableHead>
                            <TableHead>Saldo</TableHead>
                            <TableHead>Número de cuenta</TableHead>
                            <TableHead>Identificación</TableHead>
                            <TableHead className="text-right">Modificar</TableHead>
                        </TableRow>
                        </TableHeader>
                        <TableBody>
                        {filteredCards.length > 0 ? (
                            filteredCards.map((card) => (
                            <TableRow key={card.card_number}>
                                <TableCell className="font-medium">{maskCardNumber(card.card_number)}</TableCell>
                                <TableCell>{getCardTypeLabel(card.type)}</TableCell>
                                <TableCell className="font-medium">{card.balance}</TableCell>
                                <TableCell>{card.account_id}</TableCell>
                                <TableCell>{card.client_id}</TableCell>
                                <TableCell className="text-right">
                                    <div className="actions-cell">
                                        <Button variant="outline" size="sm" onClick={() => openEditCardDialog(card)}>
                                            <Pencil className="h-4 w-4" />
                                            <span className="sr-only">Editar</span>
                                        </Button>
                                        <Button
                                            variant="outline"
                                            size="sm"
                                            className="delete-button"
                                            onClick={() => handleDeleteCard(card.card_number)}
                                        >
                                            <Trash2 className="h-4 w-4" />
                                            <span className="sr-only">Eliminar</span>
                                        </Button>
                                    </div>
                                </TableCell>
                            </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={6} className="text-center py-6 text-muted-foreground">
                                    No hay tarjetas asociadas a dicha identificación
                                </TableCell>
                            </TableRow>
                        )}
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>

            {/* Dialogo o formulario para editar or agregar una nueva tarjeta */}
            <Dialog open={isCardDialogOpen} onOpenChange={setIsCardDialogOpen}>
                <DialogContent className="dialog-content">
                    <DialogHeader className="flex-shrink-0">
                        <DialogTitle>{editingCard ? "Editar tarjeta" : "Crear una nueva tarjeta"}</DialogTitle>
                        <DialogDescription>
                            {editingCard ? "Actualice los datos de la tarjeta abajo." : "Llene los datos requeridos para crear una nueva tarjeta."}
                        </DialogDescription>
                    </DialogHeader>

                    <div className="dialog-body">
                        <div className="grid gap-4">
                            {editingCard && (
                                <div className="grid gap-2">
                                    <Label>Número de tarjeta</Label>
                                    <Input value={maskCardNumber(editingCard.card_number)} disabled />
                                    <p className="card-number-mask">El número de la tarjeta no puede ser modificado</p>
                                </div>
                            )}

                            <div className="grid gap-2">
                                <Label htmlFor="type">Tipo de tarjeta</Label>
                                <Select
                                    value={String(editingCard ? editingCard.type : newCard.type)}
                                    onValueChange={(value) => {
                                    if (editingCard) {
                                        setEditingCard({ ...editingCard, type: Number(value) })
                                    } else {
                                        setNewCard({ ...newCard, type: Number(value) })
                                    }
                                }}
                                >
                                <SelectTrigger id="type">
                                    <SelectValue placeholder="Select card type" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value="1">Crédito</SelectItem>
                                    <SelectItem value="2">Débito</SelectItem>
                                </SelectContent>
                                </Select>
                            </div>

                            <div className="grid gap-2">
                                <Label htmlFor="balance">Saldo</Label>
                                <div className="balance-input-container">
                                    <DollarSign className="balance-input-icon" />
                                    <Input
                                        id="balance"
                                        type="number"
                                        step="0.01"
                                        min="0"
                                        className="pl-10"
                                        value={editingCard ? editingCard.balance : newCard.balance}
                                        onChange={(e) => {
                                        const value = e.target.value
                                        if (editingCard) {
                                            setEditingCard({ ...editingCard, balance: value })
                                        } else {
                                            setNewCard({ ...newCard, balance: value })
                                        }
                                        }}
                                        placeholder="Ingrese el saldo de la tarjeta"
                                    />
                                </div>
                            </div>

                            <div className="grid gap-2">
                                <Label htmlFor="account_id">Número de cuenta</Label>
                                <Input
                                    id="account_id"
                                    value={editingCard ? editingCard.account_id : newCard.account_id}
                                    onChange={(e) => {
                                        if (editingCard) {
                                        setEditingCard({ ...editingCard, account_id: e.target.value })
                                        } else {
                                        setNewCard({ ...newCard, account_id: e.target.value })
                                        }
                                    }}
                                    placeholder="Ingrese el número de cuenta asociada"
                                />
                                <p className="card-number-mask">Ingrese el número de cuenta al que la tarjeta estará asociada.</p>
                            </div>
                        </div>
                    </div>

                    <DialogFooter className="dialog-footer">
                        <Button variant="outline" onClick={() => setIsCardDialogOpen(false)}>
                            Cancelar
                        </Button>
                        <Button onClick={editingCard ? handleEditCard : handleAddCard}>
                            {editingCard ? "Guardar cambios" : "Crear tarjeta"}
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </div>
    )
}