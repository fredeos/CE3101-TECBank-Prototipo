import { useState, useEffect } from "react"
import { ArrowLeft, Plus, Pencil, Trash2, Search, X, DollarSign } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"
import "./gestion_cuentas.css"

export default function AccountManagement({ onBack }) {
    
    // Ejemplos de como debería ir la información de las cuentas
    const [accounts, setAccounts] = useState([
    {
        id: "ACC-10001",
        type: 1, // de ahorros
        balance: 5000.75,
        description: "Primary savings account",
        currency_id: 1, // USD
        client_id: "123456789",
        rem_state: 0,
    },
    {
        id: "ACC-10002",
        type: 2, // corriente
        balance: 2500.5,
        description: "Business checking account",
        currency_id: 1, // USD
        client_id: "CORP123456",
        rem_state: 0,
    },
    {
        id: "ACC-10003",
        type: 1, // de ahorros
        balance: 10000.0,
        description: "High-interest savings",
        currency_id: 2, // EUR
        client_id: "987654321",
        rem_state: 0,
    },
    {
        id: "ACC-10004",
        type: 2, // corriente
        balance: 7500.25,
        description: "Personal checking account",
        currency_id: 3, // CRC
        client_id: "123456789",
        rem_state: 0,
    },
    ])

    // Estados para filtrar las cuentas
    const [clientIdFilter, setClientIdFilter] = useState("")
    const [filteredAccounts, setFilteredAccounts] = useState(accounts)

    // Cambiar cuentas filtradas cuando la barra de filtrado cambie
    useEffect(() => {
        if (clientIdFilter.trim() === "") {
            setFilteredAccounts(accounts)  // muestra todas las cuentas, pues el espacio de busqueda es ""
        } else {
            // Se recorre la lista de cuentas y se busca el ID que el usuario colocó en la barra de busqueda (omite las mayusculas)
            const filtered = accounts.filter((account) => account.client_id.toLowerCase().includes(clientIdFilter.toLowerCase())) 
            setFilteredAccounts(filtered)
        }
    }, [clientIdFilter, accounts])

    // State for the account being edited
    const [editingAccount, setEditingAccount] = useState(null)
    const [isAccountDialogOpen, setIsAccountDialogOpen] = useState(false)
    const [newAccount, setNewAccount] = useState({
        id: "",
        type: 1,
        balance: "",
        description: "",
        currency_id: 1,
        client_id: "",
        rem_state: 0,
    })
    
    //Simula la generación de numero de cuenta (lo generará el frontend)
    const generateAccountId = () => {
        const lastId = accounts.length > 0 ? Number.parseInt(accounts[accounts.length - 1].id.split("-")[1]) : 10000
        return `ACC-${lastId + 1}`
    }

    // Función paraagregar una cuenta nueva
    const handleAddAccount = () => {
        const id = generateAccountId()

        // Convertir el ingreso mensual o balance en numero (se obtiene de una entrad de texto)
        const balance = newAccount.balance === "" ? 0 : Number.parseFloat(newAccount.balance)
        setAccounts([...accounts, { ...newAccount, id: id, balance: balance }])
        setNewAccount({
            id: "",
            type: 1,
            balance: "",
            description: "",
            currency_id: 1,
            client_id: "",
            rem_state: 0,
        })
        setIsAccountDialogOpen(false)
    }

    // Función para editar una cuenta
    const handleEditAccount = () => {
        // Convert balance to number before saving
        const balance = editingAccount.balance === "" ? 0 : Number.parseFloat(editingAccount.balance)
        const updatedAccount = { ...editingAccount, balance }

        setAccounts(accounts.map((account) => (account.id === updatedAccount.id ? updatedAccount : account)))
        setEditingAccount(null)
        setIsAccountDialogOpen(false)
    }

    // Función para eliminar una cuenta
    const handleDeleteAccount = (id) => {
        setAccounts(accounts.filter((account) => account.id !== id))
    }

    // Función para abrir el formulario y agregar una nueva cuenta
    const openAddAccountDialog = () => {
        setEditingAccount(null)
        setNewAccount({
            id: "",
            type: 1,
            balance: "",
            description: "",
            currency_id: 1,
            client_id: "",
            rem_state: 0,
        })
        setIsAccountDialogOpen(true)
    }

    // Función para abrir el formulario y editar una cuenta
    const openEditAccountDialog = (account) => {
        setEditingAccount({ ...account })
        setIsAccountDialogOpen(true)
    }

    // Función para limpiar la barra de busqueda o filtro cuando se presiona el boton X
    const clearFilter = () => {
        setClientIdFilter("")
    }

    // Función para obtener el label del tipo de cuenta (texto que se observa en la tabla)
    const getAccountTypeLabel = (type) => {
        return type === 1 ? "Savings" : "Checking"
    }

    // Función pára obtener el label de la moneda (se observa en la tabla)
    const getCurrencyTypeLabel = (type) => {
        const currencies = {
            1: "USD",
            2: "EUR",
            3: "CRC",
        }
        return currencies[type] || "Unknown"
    }

    return (
        <div className="account-management-container">
            <div className="header-container">
                <Button variant="ghost" className="mr-4" onClick={onBack}>
                    <ArrowLeft className="mr-2 h-4 w-4" /> Volver al panel de control
                </Button>
                <h1 className="header-title">Gestión de Cuentas</h1>
            </div>

            <div className="title-container">
                <p className="text-muted-foreground">Administración de cuentas bancarias y su información.</p>
                <Button onClick={openAddAccountDialog}>
                    <Plus className="mr-2 h-4 w-4" /> Crear nueva cuenta
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
                {filteredAccounts.length === 0 && (
                    <p className="no-accounts-message">Sin resultados</p>
                )}
            </div>

            <Card>
                <CardContent className="p-0">
                <Table className="account-table">
                    <TableHeader>
                        <TableRow>
                            <TableHead className="account-table-header">Número de cuenta</TableHead>
                            <TableHead className="account-table-header">Tipo</TableHead>
                            <TableHead className="account-table-header">Saldo</TableHead>
                            <TableHead className="account-table-header">Moneda</TableHead>
                            <TableHead className="account-table-header">Identificación</TableHead>
                            <TableHead className="account-table-header">Descripción</TableHead>
                            <TableHead className="account-table-header text-right">Modificar</TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {filteredAccounts.length > 0 ? (
                            filteredAccounts.map((account) => (
                                <TableRow key={account.id}>
                                    <TableCell className="account-table-cell font-medium">{account.id}</TableCell>
                                    <TableCell className="account-table-cell"> {getAccountTypeLabel(account.type)} </TableCell>
                                    <TableCell className="account-table-cell font-medium">{account.balance} </TableCell>
                                    <TableCell className="account-table-cell">{getCurrencyTypeLabel(account.currency_id)}</TableCell>
                                    <TableCell className="account-table-cell">{account.client_id}</TableCell>
                                    <TableCell className="account-table-cell truncated-description" title={account.description}> {account.description} </TableCell>
                                    <TableCell className="account-table-cell actions-cell">
                                        <div className="actions-container">
                                            <Button variant="outline" size="sm" onClick={() => openEditAccountDialog(account)}>
                                                <Pencil className="h-4 w-4" />
                                                <span className="sr-only">Editar</span>
                                            </Button>
                                            <Button
                                                variant="outline"
                                                size="sm"
                                                className="text-red-500 hover:text-red-600"
                                                onClick={() => handleDeleteAccount(account.id)}
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
                                <TableCell colSpan={7} className="account-table-cell text-center py-6 text-muted-foreground">
                                    No hay cuentas asociadas a dicha identificación
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
                </CardContent>
            </Card>

            {/* Dialogo o formulario para editar o agregar una nueva cuenta */}
            <Dialog open={isAccountDialogOpen} onOpenChange={setIsAccountDialogOpen}>
                <DialogContent className="account-dialog">
                    <DialogHeader className="flex-shrink-0">
                        <DialogTitle>{editingAccount ? "Editar cuenta" : "Crear una nueva cuenta"}</DialogTitle>
                        <DialogDescription>
                            {editingAccount ? "Actualice los datos de la cuenta abajo." : "Llene los datos requeridos para crear una nueva cuenta."}
                        </DialogDescription>
                    </DialogHeader>

                    <div className="dialog-content-container">
                        <div className="dialog-grid">
                            {/* Condición de que si se está editando, entonces se muestra el numero de cuenta (pero desabilitado) */}
                            {editingAccount && (
                                <div className="grid gap-2">
                                    <Label>Número de cuenta</Label>
                                    <Input value={editingAccount.id} disabled />
                                    <p className="text-xs text-muted-foreground">Número de cuenta no puede ser modificado</p>
                                </div>
                            )}

                            <div className="dialog-subgrid">
                                <div className="grid gap-2">
                                <Label htmlFor="type">Tipo de cuenta</Label>
                                <Select
                                    value={String(editingAccount ? editingAccount.type : newAccount.type)}
                                    onValueChange={(value) => {
                                    if (editingAccount) {
                                        setEditingAccount({ ...editingAccount, type: Number(value) })
                                    } else {
                                        setNewAccount({ ...newAccount, type: Number(value) })
                                    }
                                    }}
                                >
                                    <SelectTrigger id="type">
                                        <SelectValue placeholder="Seleccione un tipo de cuenta" />
                                    </SelectTrigger>
                                    <SelectContent>
                                        <SelectItem value="1">Cuenta de ahorro</SelectItem>
                                        <SelectItem value="2">Cuenta corriente</SelectItem>
                                    </SelectContent>
                                </Select>
                                </div>

                                <div className="grid gap-2">
                                    <Label htmlFor="currency_id">Moneda</Label>
                                    <Select
                                        value={String(editingAccount ? editingAccount.currency_id : newAccount.currency_id)}
                                        onValueChange={(value) => {
                                        if (editingAccount) {
                                            setEditingAccount({ ...editingAccount, currency_id: Number(value) })
                                        } else {
                                            setNewAccount({ ...newAccount, currency_id: Number(value) })
                                        }
                                        }}
                                    >
                                        <SelectTrigger>
                                            <SelectValue placeholder="Seleccione la moneda" />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="1">USD </SelectItem>
                                            <SelectItem value="2">EUR </SelectItem>
                                            <SelectItem value="3">CRC </SelectItem>
                                        </SelectContent>
                                    </Select>
                                </div>
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
                                        value={editingAccount ? editingAccount.balance : newAccount.balance}
                                        onChange={(e) => {
                                        const value = e.target.value
                                        if (editingAccount) {
                                            setEditingAccount({ ...editingAccount, balance: value })
                                        } else {
                                            setNewAccount({ ...newAccount, balance: value })
                                        }
                                        }}
                                        placeholder="Ingrese un monto"
                                    />
                                    </div>
                            </div>

                            <div className="grid gap-2">
                                <Label htmlFor="client_id">Número de identificación (Cédula)</Label>
                                <Input
                                id="client_id"
                                value={editingAccount ? editingAccount.client_id : newAccount.client_id}
                                onChange={(e) => {
                                    if (editingAccount) {
                                    setEditingAccount({ ...editingAccount, client_id: e.target.value })
                                    } else {
                                    setNewAccount({ ...newAccount, client_id: e.target.value })
                                    }
                                }}
                                placeholder="Ingrese el número de identificación del cliente"
                                />
                            </div>

                            <div className="grid gap-2">
                                <Label htmlFor="description">Descripción</Label>
                                <Textarea
                                    id="description"
                                    value={editingAccount ? editingAccount.description : newAccount.description}
                                    onChange={(e) => {
                                        if (editingAccount) {
                                            setEditingAccount({ ...editingAccount, description: e.target.value })
                                        } 
                                        else {
                                            setNewAccount({ ...newAccount, description: e.target.value })
                                        }
                                    }}
                                    placeholder="Ingrese el proposito de la cuenta"
                                />
                            </div>
                        </div>
                    </div>

                    <DialogFooter className="dialog-footer">
                        <Button variant="outline" onClick={() => setIsAccountDialogOpen(false)}>
                            Cancelar
                        </Button>
                        <Button onClick={editingAccount ? handleEditAccount : handleAddAccount}>
                            {editingAccount ? "Guardar cambios" : "Crear cuenta"}
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </div>
    )
}