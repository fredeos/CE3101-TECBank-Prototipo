// This file is not directly used but shows how you could separate the role management into its own component
import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { ArrowLeft, Plus, Pencil, Trash2, AlignCenter } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent } from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import "./gestion_roles.css"

export default function RoleManagement({ onBack }) {
    
    // Navegar por direcciones
    const navigate = useNavigate()

    // Ejemplos de datos de los roles
    const [roles, setRoles] = useState([
        {id: 1, name: "Administrador", description: "Tiene asesor a todas las funciones del sistema"},
        {id: 2, name: "Asesor de ventas", description: "Brinda préstamos a los clientes y asesoría con respecto a ello"},
        {id: 3, name: "Contador", description: "Se encarga de temas económicos internos del banco"},
        {id: 4, name: "Abogado", description: "Se encarga de temas legales del banco, tanto para servicios de clientes como propios del banco"},
        {id: 5, name: "Guarda", description: "Se encarga de la seguridad del banco, en sus instalaciones"}
    ])

    // Control de estados para cuando se edita algun rol o se agrega uno nuevo
    const [editingRole, setEditingRole] = useState(null)
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [newRole, setNewRole] = useState({ name: "", description: ""})

    // Función para agregar un nuevo rol en la lista, genera un ID de acuerdo a los que existan
    const handleAddRole = () => {
        const id = roles.length > 0 ? Math.max(...roles.map((role) => role.id)) + 1 : 1
        setRoles([...roles, { id, ...newRole }])  // Hace una copia de los roles existentes y concatena el id generado con la información del nuevo rol, y eso se agrega con la copia de los roles
        setNewRole({ name: "", description: ""})
        setIsDialogOpen(false)
    }

    // Función para colocar dentro de la lista de roles, el rol editado
    const handleEditRole = () => {
        setRoles(roles.map((role) => (role.id === editingRole.id ? editingRole : role))) // Se recorre toda
        setEditingRole(null)
        setIsDialogOpen(false)
    }

    // Función para eliminar rol
    const handleDeleteRole = (id) => {
        setRoles(roles.filter((role) => role.id !== id))
    }

    // Función para abrir el formulario donde se agregará un nuevo rol
    const openAddDialog = () => {
        setEditingRole(null)
        setNewRole({ name: "", description: ""})
        setIsDialogOpen(true)
    }

    // Función para abrir el formulario donde se modificará un rol existente
    const openEditDialog = (role) => {
        setEditingRole({ ...role })
        setIsDialogOpen(true)
    }

    return (
    <div className="role-management-container">
        <div className="role-header">
            <Button variant="ghost" className="mr-4" onClick={() => navigate("/adminDashboard")}>
                <ArrowLeft className="mr-2 h-4 w-4" /> Volver al panel de control
            </Button>
            <h1 className="role-title">Gestion de Roles</h1>
        </div>

        <div className="role-subheader">
            <p className="role-description">Panel para la adición, modificación y la eliminación de roles de la compañia.</p>
            <Button onClick={openAddDialog}>
                <Plus className="mr-2 h-4 w-4" />Agregar nuevo Rol
            </Button>
        </div>

        <Card>
            <CardContent className="p-0">
                <Table>
                    {/* Encabezado de la Tabla */}
                    <TableHeader>
                        <TableRow>
                            {/* Se modifica cada encabezado de celdas individuales */}
                            <TableHead className="cell-header1">Rol</TableHead>
                            <TableHead className="cell-header1">Descripción</TableHead>
                            <TableHead className="cell-header2">Modificar</TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {/* Se realiza una especie de iteración para cada rol en 'roles' y crear una fila para cada una */}
                        {roles.map((role) => (
                            <TableRow key={role.id}>
                                <TableCell className="table-cell-role">{role.name}</TableCell>
                                <TableCell>{role.description}</TableCell>
                                <TableCell>
                                    <div className="actions-buttons">
                                        <Button variant="outline" size="sm" onClick={() => openEditDialog(role)}>
                                            <Pencil className="h-4 w-4" />
                                            <span className="sr-only">Edit</span>
                                        </Button>
                                        <Button variant="outline" size="sm" className="delete-button" onClick={() => handleDeleteRole(role.id)}>
                                            <Trash2 className="h-4 w-4" />
                                            <span className="sr-only">Delete</span>
                                        </Button>
                                    </div>
                                </TableCell>
                            </TableRow>
                            ))
                        }
                    </TableBody>
                </Table>
            </CardContent>
        </Card>

        {/* Formulario donde se agrega o se modifica un rol */}
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
            <DialogContent>
                <DialogHeader>
                    <DialogTitle>{editingRole ? "Editar Rol" : "Agregar Nuevo Rol"}</DialogTitle>
                    <DialogDescription>
                        {editingRole ? "Update the role details below." : "Llene los espacios para agregar un rol."}
                    </DialogDescription>
                </DialogHeader>

                <div className="grid gap-4 py-4">

                    {/* Espacio de ingreso o modificación del nombre del rol en el formulario */}
                    <div className="grid gap-2">
                        <Label htmlFor="name">Rol</Label>
                        <Input id="name"
                        value={editingRole ? editingRole.name : newRole.name}
                        onChange={(e) => {
                            if (editingRole) {
                            setEditingRole({ ...editingRole, name: e.target.value })
                            } else {
                            setNewRole({ ...newRole, name: e.target.value })
                            }
                        }}
                        />
                    </div>
                    
                    {/* Espacio de ingreso o modificación de la descripción del rol en el formulario */}
                    <div className="grid gap-2">
                        <Label htmlFor="description">Descripción</Label>
                        <Input
                        id="description"
                        value={editingRole ? editingRole.description : newRole.description} 
                        onChange={(e) => {
                            if (editingRole) {
                            setEditingRole({ ...editingRole, description: e.target.value })
                            } else {
                            setNewRole({ ...newRole, description: e.target.value })
                            }
                        }}
                        />
                    </div>
                </div>
                
                {/* Pie del fomrulario donde se encuentran los botones para guardar o cancelar */}
                <DialogFooter>
                    <Button variant="outline" onClick={() => setIsDialogOpen(false)}>
                        Cancelar
                    </Button>
                    <Button onClick={editingRole ? handleEditRole : handleAddRole}>
                        {editingRole ? "Guardar cambios" : "Agregar Rol"}
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    </div>
    )
}

// Estructura para comentarios  --->  {/* comentario */}