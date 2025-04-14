import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { ArrowLeft, Plus, Pencil, Trash2, Eye, EyeOff } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"
import "./gestion_clientes.css"

export default function ClientManagement({ onBack }) {

    // Navegar por direcciones
    const navigate = useNavigate()

    // Ejemplos de datos de clientes
    const [clients, setClients] = useState([
        {
            id: 1,
            name: "John",
            firstLastName: "Smith",
            secondLastName: "Johnson",
            identificationNumber: "123456789",
            address: "123 Main St, New York, NY 10001",
            phoneNumber: "555-123-4567",
            salary: 75000,
            clientType: "normal",
            username: "johnsmith",
            password: "password123",
        },
        {
            id: 2,
            name: "Maria",
            firstLastName: "Garcia",
            secondLastName: "Rodriguez",
            identificationNumber: "987654321",
            address: "456 Park Ave, Miami, FL 33101",
            phoneNumber: "555-987-6543",
            salary: 82000,
            clientType: "normal",
            username: "mariagarcia",
            password: "password456",
        },
        {
            id: 3,
            name: "Acme",
            firstLastName: "Corporation",
            secondLastName: "",
            identificationNumber: "CORP123456",
            address: "789 Business Blvd, Chicago, IL 60601",
            phoneNumber: "506-7890-0123",
            salary: 0,
            clientType: "legal",
            username: "acmecorp",
            password: "password789",
        }
    ])

    // Control de estados para cuando se edita algun cliente o se agrega uno nuevo
    const [editingClient, setEditingClient] = useState(null)
    const [isClientDialogOpen, setIsClientDialogOpen] = useState(false)
    const [newClient, setNewClient] = useState({
        name: "",
        firstLastName: "",
        secondLastName: "",
        identificationNumber: "",
        address: "",
        phoneNumber: "",
        salary: "",
        clientType: "",
        username: "",
        password: "",
    })
    const [showPassword, setShowPassword] = useState(false)

    // Función de estados para cuando se agrega un nuevo cliente en la lista
    const handleAddClient = () => {
        const id = clients.length > 0 ? Math.max(...clients.map((client) => client.id)) + 1 : 1
        setClients([...clients, { id, ...newClient }])
        setNewClient({
            name: "",
            firstLastName: "",
            secondLastName: "",
            identificationNumber: "",
            address: "",
            phoneNumber: "",
            salary: "",
            clientType: "",
            username: "",
            password: "",
        })
        setIsClientDialogOpen(false)
    }

    // Función para colocar dentro de la lista de clientes, el cliente editado
    const handleEditClient = () => {
        setClients(clients.map((client) => (client.id === editingClient.id ? editingClient : client)))
        setEditingClient(null)
        setIsClientDialogOpen(false)
    }

    // Función para eliminar un cliente
    const handleDeleteClient = (id) => {
        setClients(clients.filter((client) => client.id !== id))
    }

    // Función para abrir el formulario de adición de un cliente
    const openAddClientDialog = () => {
        setEditingClient(null)
        setNewClient({
            name: "",
            firstLastName: "",
            secondLastName: "",
            identificationNumber: "",
            address: "",
            phoneNumber: "",
            salary: "",
            clientType: "",
            username: "",
            password: "",
        })
        setIsClientDialogOpen(true)
        setShowPassword(false)
    }

    // Función para abrir el formulario de edición de un cliente
    const openEditClientDialog = (client) => {
        setEditingClient({ ...client })
        setIsClientDialogOpen(true)
        setShowPassword(false)
    }

    return (
        <div className="client-management-container">
            <div className="header-container">
                <Button variant="ghost" onClick={() => navigate("/adminDashboard")}>
                    <ArrowLeft className="mr-2 h-4 w-4" /> Volver al panel de control
                </Button>
                <h1 className="header-title">Gestion de Clientes</h1>
            </div>

            <div className="controls-container">
                <p className="description-text">Panel para la adición, modificación y la eliminación de clientes de la compañia</p>
                <Button onClick={openAddClientDialog}>
                <Plus className="mr-2 h-4 w-4" /> Agregar un nuevo cliente
                </Button>
            </div>

            <Card>
                <CardContent className="p-0">
                    <Table className="client-table">
                        {/* Encabezado de la Tabla */}
                        <TableHeader>
                            <TableRow>
                                {/* Se modifica cada encabezado de celdas individuales */}
                                <TableHead className="header-table1">Nombre</TableHead>
                                <TableHead className="header-table1">Cédula</TableHead>
                                <TableHead className="header-table1">Tipo de cliente</TableHead>
                                <TableHead className="header-table1">Usuario</TableHead>
                                <TableHead className="header-table1">Teléfono</TableHead>
                                <TableHead className="header-table2">Modificación</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {/* Se realiza una especie de iteración para cada Cliente en 'client' y crear una fila para cada uno */}
                            {clients.map((client) => (
                            <TableRow key={client.id}>
                                <TableCell className="font-medium">
                                    {client.name} {client.firstLastName} {client.secondLastName}
                                </TableCell>
                                <TableCell>{client.identificationNumber}</TableCell>
                                <TableCell className="capitalize">{client.clientType}</TableCell>
                                <TableCell>{client.username}</TableCell>
                                <TableCell>{client.phoneNumber}</TableCell>
                                <TableCell className="action-buttons">
                                    <div className="actions-container">
                                        <Button variant="outline" size="sm" onClick={() => openEditClientDialog(client)}>
                                            <Pencil className="h-4 w-4" />
                                            <span className="sr-only">Edit</span>
                                        </Button>
                                        <Button
                                            variant="outline"
                                            size="sm"
                                            className="delete-button"
                                            onClick={() => handleDeleteClient(client.id)}
                                        >
                                            <Trash2 className="h-4 w-4" />
                                            <span className="sr-only">Delete</span>
                                        </Button>
                                    </div>
                                </TableCell>
                            </TableRow>
                        ))}
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>

            {/* Formulario donde se agrega o se modifica un cliente */}
            <Dialog open={isClientDialogOpen} onOpenChange={setIsClientDialogOpen}>
                <DialogContent className="client-dialog">
                    <DialogHeader className="flex-shrink-0">
                        <DialogTitle>{editingClient ? "Editar Cliente" : "Agregar nuevo cliente"}</DialogTitle>
                        <DialogDescription>
                            {editingClient ? "Actualizar los datos abajo." : "Llene los espacios para agregar a un cliente."}
                        </DialogDescription>
                    </DialogHeader>

                    <div className="dialog-content">
                        <div className="grid gap-4">

                            {/* Espacio de ingreso o modificación del nombre completo del cliente */}
                            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                                <div className="grid gap-2">
                                    <Label htmlFor="name">Nombre</Label>
                                    <Input
                                        id="name"
                                        value={editingClient ? editingClient.name : newClient.name}
                                        onChange={(e) => {
                                        if (editingClient) {
                                            setEditingClient({ ...editingClient, name: e.target.value })
                                        } else {
                                            setNewClient({ ...newClient, name: e.target.value })
                                        }
                                        }}
                                    />
                                </div>

                                <div className="grid gap-2">
                                    <Label htmlFor="firstLastName">Primer apellido</Label>
                                    <Input
                                        id="firstLastName"
                                        value={editingClient ? editingClient.firstLastName : newClient.firstLastName}
                                        onChange={(e) => {
                                        if (editingClient) {
                                            setEditingClient({ ...editingClient, firstLastName: e.target.value })
                                        } else {
                                            setNewClient({ ...newClient, firstLastName: e.target.value })
                                        }
                                        }}
                                    />
                                </div>

                                <div className="grid gap-2">
                                    <Label htmlFor="secondLastName">Segundo apellido</Label>
                                    <Input
                                        id="secondLastName"
                                        value={editingClient ? editingClient.secondLastName : newClient.secondLastName}
                                        onChange={(e) => {
                                        if (editingClient) {
                                            setEditingClient({ ...editingClient, secondLastName: e.target.value })
                                        } else {
                                            setNewClient({ ...newClient, secondLastName: e.target.value })
                                        }
                                        }}
                                    />
                                </div>
                            </div>

                            {/* Espacio de ingreso o modificación de la cédula del cliente */}
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                <div className="grid gap-2">
                                    <Label htmlFor="identificationNumber">Cédula</Label>
                                    <Input
                                        id="identificationNumber"
                                        value={editingClient ? editingClient.identificationNumber : newClient.identificationNumber}
                                        onChange={(e) => {
                                        if (editingClient) {
                                            setEditingClient({ ...editingClient, identificationNumber: e.target.value })
                                        } else {
                                            setNewClient({ ...newClient, identificationNumber: e.target.value })
                                        }
                                        }}
                                    />
                                </div>
                                
                                <div className="grid gap-2">
                                    <Label htmlFor="clientType">Tipo de cliente</Label>
                                    <Select
                                        value={editingClient ? editingClient.clientType : newClient.clientType}
                                        onValueChange={(value) => {
                                            if (editingClient) {
                                                setEditingClient({ ...editingClient, clientType: value })
                                            } else {
                                                setNewClient({ ...newClient, clientType: value })
                                            }
                                        }}
                                    >
                                        <SelectTrigger>
                                            <SelectValue placeholder="Seleccionar tipo de cliente" />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="normal">Físico</SelectItem>
                                            <SelectItem value="legal">Jurídico</SelectItem>
                                        </SelectContent>
                                    </Select>
                                </div>
                            </div>
                            
                            <div className="form-single-inputs">
                                <Label htmlFor="address">Dirección residencial</Label>
                                <Textarea
                                    id="address"
                                    value={editingClient ? editingClient.address : newClient.address}
                                    onChange={(e) => {
                                        if (editingClient) {
                                        setEditingClient({ ...editingClient, address: e.target.value })
                                        } else {
                                        setNewClient({ ...newClient, address: e.target.value })
                                        }
                                    }}
                                />
                            </div>

                            
                            <div className="form-single-inputs">
                                <Label htmlFor="phoneNumber">Número de teléfonico</Label>
                                <Input
                                    id="phoneNumber"
                                    value={editingClient ? editingClient.phoneNumber : newClient.phoneNumber}
                                    onChange={(e) => {
                                    if (editingClient) {
                                        setEditingClient({ ...editingClient, phoneNumber: e.target.value })
                                    } else {
                                        setNewClient({ ...newClient, phoneNumber: e.target.value })
                                    }
                                    }}
                                />
                            </div>

                            <div className="form-single-inputs">
                                <Label htmlFor="salary">Ingreso mensual</Label>
                                <Input
                                    id="salary"
                                    type="number"
                                    value={editingClient ? editingClient.salary : newClient.salary}
                                    onChange={(e) => {
                                    if (editingClient) {
                                        setEditingClient({ ...editingClient, salary: e.target.value })
                                    } else {
                                        setNewClient({ ...newClient, salary: e.target.value })
                                    }
                                    }}
                                />
                            </div>
                            

                            <div className="form-single-inputs">
                                <Label htmlFor="username">Usuario</Label>
                                <Input
                                id="username"
                                value={editingClient ? editingClient.username : newClient.username}
                                onChange={(e) => {
                                    if (editingClient) {
                                    setEditingClient({ ...editingClient, username: e.target.value })
                                    } else {
                                    setNewClient({ ...newClient, username: e.target.value })
                                    }
                                }}
                                />
                            </div>

                            <div className="form-single-inputs">
                                <Label htmlFor="password">Contraseña</Label>
                                <div className="password-input-container">
                                    <Input
                                        id="password"
                                        type={showPassword ? "text" : "password"}
                                        value={editingClient ? editingClient.password : newClient.password}
                                        onChange={(e) => {
                                        if (editingClient) {
                                            setEditingClient({ ...editingClient, password: e.target.value })
                                        } else {
                                            setNewClient({ ...newClient, password: e.target.value })
                                        }
                                        }}
                                    />
                                    <Button
                                        type="button"
                                        variant="ghost"
                                        size="icon"
                                        className="password-toggle-button"
                                        onClick={() => setShowPassword(!showPassword)}
                                    >
                                        {showPassword ? <Eye className="h-4 w-4" /> : <EyeOff className="h-4 w-4" />}
                                        <span className="sr-only">{showPassword ? "Hide password" : "Show password"}</span>
                                    </Button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <DialogFooter className="dialog-footer">
                        <Button variant="outline" onClick={() => setIsClientDialogOpen(false)}>
                            Cancelar
                        </Button>
                        <Button onClick={editingClient ? handleEditClient : handleAddClient}>
                            {editingClient ? "Guardar cambios" : "Agregar cliente"}
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </div>
    )
}
