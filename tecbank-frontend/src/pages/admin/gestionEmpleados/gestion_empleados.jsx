import { useState, useEffect } from "react"
import { ArrowLeft, Plus, Pencil, Trash2, Search, X } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Badge } from "@/components/ui/badge"
import "./gestion_empleados.css"

export default function EmployeeManagement({ onBack }) {
    // Sample roles data
    const [roles, setRoles] = useState([
        { id: 1, name: "Administrator", description: "Full access to all system functions" },
        { id: 2, name: "Manager", description: "Access to manage users and view reports" },
        { id: 3, name: "Teller", description: "Process customer transactions" },
        { id: 4, name: "Auditor", description: "View-only access to transactions and reports" },
        { id: 5, name: "Customer Service", description: "Handle customer inquiries and basic account management" },
    ])

    // Sample employees data
    const [employees, setEmployees] = useState([
        {
        id: 1,
        clientId: "E-123456789",
        name: "John",
        firstLastName: "Smith",
        secondLastName: "Johnson",
        roleId: 1,
        },
        {
        id: 2,
        clientId: "E-987654321",
        name: "Maria",
        firstLastName: "Garcia",
        secondLastName: "Rodriguez",
        roleId: 2,
        },
        {
        id: 3,
        clientId: "E-456789123",
        name: "Robert",
        firstLastName: "Williams",
        secondLastName: "Brown",
        roleId: 3,
        },
        {
        id: 4,
        clientId: "E-789123456",
        name: "Sarah",
        firstLastName: "Davis",
        secondLastName: "Miller",
        roleId: 4,
        },
        {
        id: 5,
        clientId: "E-321654987",
        name: "Michael",
        firstLastName: "Taylor",
        secondLastName: "Wilson",
        roleId: 5,
        },
    ])

    // State for filtering
    const [employeeIdFilter, setEmployeeIdFilter] = useState("")
    const [filteredEmployees, setFilteredEmployees] = useState(employees)

    // Update filtered employees when filter changes
    useEffect(() => {
        if (employeeIdFilter.trim() === "") {
            setFilteredEmployees(employees)
        } 
        else {
        const filtered = employees.filter((employee) =>
            employee.clientId.toLowerCase().includes(employeeIdFilter.toLowerCase()),
        )
            setFilteredEmployees(filtered)
        }
    }, [employeeIdFilter, employees])

    // State for the employee being edited
    const [editingEmployee, setEditingEmployee] = useState(null)
    const [isEmployeeDialogOpen, setIsEmployeeDialogOpen] = useState(false)
    const [newEmployee, setNewEmployee] = useState({
        clientId: "",
        name: "",
        firstLastName: "",
        secondLastName: "",
        roleId: "",
    })

    // Function to handle adding a new employee
    const handleAddEmployee = () => {
        const id = employees.length > 0 ? Math.max(...employees.map((employee) => employee.id)) + 1 : 1
        setEmployees([...employees, { id, ...newEmployee }])
        setNewEmployee({
            clientId: "",
            name: "",
            firstLastName: "",
            secondLastName: "",
            roleId: "",
        })
        setIsEmployeeDialogOpen(false)
    }

    // Function to handle editing an employee
    const handleEditEmployee = () => {
        setEmployees(employees.map((employee) => (employee.id === editingEmployee.id ? editingEmployee : employee)))
        setEditingEmployee(null)
        setIsEmployeeDialogOpen(false)
    }

    // Function to handle deleting an employee
    const handleDeleteEmployee = (id) => {
        setEmployees(employees.filter((employee) => employee.id !== id))
    }

    // Function to open the dialog for adding a new employee
    const openAddEmployeeDialog = () => {
        setEditingEmployee(null)
        setNewEmployee({
            clientId: "",
            name: "",
            firstLastName: "",
            secondLastName: "",
            roleId: "",
        })
        setIsEmployeeDialogOpen(true)
    }

    // Function to open the dialog for editing an employee
    const openEditEmployeeDialog = (employee) => {
        setEditingEmployee({ ...employee })
        setIsEmployeeDialogOpen(true)
    }

    // Function to clear the filter
    const clearFilter = () => {
        setEmployeeIdFilter("")
    }

    // Function to get role name by ID
    const getRoleName = (roleId) => {
        const role = roles.find((role) => role.id === roleId)
        return role ? role.name : "Unknown Role"
    }

    return (
        <div className="employee-management-container">
            <div className="header-container">
                <Button variant="ghost" className="mr-4" onClick={onBack}>
                    <ArrowLeft className="mr-2 h-4 w-4" /> Volver al panel de control
                </Button>
                <h1 className="header-title">Gestión de empleados</h1>
            </div>

            <div className="title-container">
                <p className="text-muted-foreground">Administración de empleados y su información.</p>
                <Button onClick={openAddEmployeeDialog}>
                    <Plus className="mr-2 h-4 w-4" /> Agregar nuevo empleado
                </Button>
            </div>

            {/* Barra de búsqueda por ID de empleado (filtro) */}
            <div className="search-container">
                <div className="search-input-container">
                    <Search className="search-icon" />
                    <Input
                        placeholder="Buscar por identificación de empleado..."
                        value={employeeIdFilter}
                        onChange={(e) => setEmployeeIdFilter(e.target.value)}
                        className="pl-10 pr-10"
                    />
                    {employeeIdFilter && (
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
                {filteredEmployees.length === 0 && (
                    <p className="no-results-message">Sin resultados</p>
                )}
            </div>

            <Card>
                <CardContent className="p-0">
                <Table>
                    <TableHeader>
                        <TableRow>
                            <TableHead>Identificación</TableHead>
                            <TableHead>Nombre</TableHead>
                            <TableHead>Rol</TableHead>
                            <TableHead className="actions-cell">Modificar</TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {filteredEmployees.length > 0 ? (
                            filteredEmployees.map((employee) => (
                            <TableRow key={employee.id}>
                                <TableCell className="font-medium">{employee.clientId}</TableCell>
                                <TableCell>
                                    {employee.name} {employee.firstLastName} {employee.secondLastName}
                                </TableCell>
                                <TableCell>{getRoleName(employee.roleId)}</TableCell>
                                <TableCell className="actions-cell">
                                <div className="actions-container">
                                    <Button variant="outline" size="sm" onClick={() => openEditEmployeeDialog(employee)}>
                                        <Pencil className="h-4 w-4" />
                                        <span className="sr-only">Editar</span>
                                    </Button>
                                    <Button
                                        variant="outline"
                                        size="sm"
                                        className="delete-button"
                                        onClick={() => handleDeleteEmployee(employee.id)}
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
                                <TableCell colSpan={4} className="empty-table-message">
                                    No hay empleados asociados a dicha identificación
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
                </CardContent>
            </Card>

            {/* Dialogo o formulario para editar o agregar un nuevo empleado */}
            <Dialog open={isEmployeeDialogOpen} onOpenChange={setIsEmployeeDialogOpen}>
                <DialogContent className="dialog-content">
                    <DialogHeader className="flex-shrink-0">
                        <DialogTitle>{editingEmployee ? "Editar empleado" : "Agregar nuevo empleado"}</DialogTitle>
                        <DialogDescription>
                            {editingEmployee ? "Actualice los datos del empleado abajo." : "Llene los datos requeridos para crear una nuevo empleado."}
                        </DialogDescription>
                    </DialogHeader>
                    
                    <div className="dialog-scrollable">
                        <div className="form-grid">
                            <div className="grid gap-2">
                                <Label htmlFor="clientId">Identificación de empleado (Cédula)</Label>
                                <Input
                                    id="clientId"
                                    value={editingEmployee ? editingEmployee.clientId : newEmployee.clientId}
                                    onChange={(e) => {
                                        if (editingEmployee) {
                                            setEditingEmployee({ ...editingEmployee, clientId: e.target.value })
                                        } 
                                        else {
                                            setNewEmployee({ ...newEmployee, clientId: e.target.value })
                                        }
                                    }}
                                    placeholder="Ingrese la identificación del empleado"
                                />
                            </div>

                            <div className="name-grid">
                                <div className="grid gap-2">
                                <Label htmlFor="name">Nombre</Label>
                                <Input
                                    id="name"
                                    value={editingEmployee ? editingEmployee.name : newEmployee.name}
                                    onChange={(e) => {
                                    if (editingEmployee) {
                                        setEditingEmployee({ ...editingEmployee, name: e.target.value })
                                    } else {
                                        setNewEmployee({ ...newEmployee, name: e.target.value })
                                    }
                                    }}
                                    placeholder="Ingrese el nombre"
                                />
                                </div>

                                <div className="grid gap-2">
                                    <Label htmlFor="firstLastName">Primer apellido</Label>
                                    <Input
                                        id="firstLastName"
                                        value={editingEmployee ? editingEmployee.firstLastName : newEmployee.firstLastName}
                                        onChange={(e) => {
                                        if (editingEmployee) {
                                            setEditingEmployee({ ...editingEmployee, firstLastName: e.target.value })
                                        } else {
                                            setNewEmployee({ ...newEmployee, firstLastName: e.target.value })
                                        }
                                        }}
                                        placeholder="Ingrese primer apellido"
                                    />
                                </div>

                                <div className="grid gap-2">
                                    <Label htmlFor="secondLastName">Segundo apellido</Label>
                                    <Input
                                        id="secondLastName"
                                        value={editingEmployee ? editingEmployee.secondLastName : newEmployee.secondLastName}
                                        onChange={(e) => {
                                        if (editingEmployee) {
                                            setEditingEmployee({ ...editingEmployee, secondLastName: e.target.value })
                                        } else {
                                            setNewEmployee({ ...newEmployee, secondLastName: e.target.value })
                                        }
                                        }}
                                        placeholder="Ingrese segundo apellido"
                                    />
                                </div>
                            </div>

                            <div className="grid gap-2">
                                <Label htmlFor="roleId">Rol</Label>
                                <Select
                                value={editingEmployee ? editingEmployee.roleId : newEmployee.roleId}
                                onValueChange={(value) => {
                                    if (editingEmployee) {
                                        setEditingEmployee({ ...editingEmployee, roleId: Number(value) })
                                    } else {
                                        setNewEmployee({ ...newEmployee, roleId: Number(value) })
                                    }
                                }}
                                >
                                <SelectTrigger id="roleId">
                                    <SelectValue placeholder="Seleccione rol del empleado" />
                                </SelectTrigger>
                                <SelectContent>
                                    {roles.map((role) => (
                                        <SelectItem key={role.id} value={role.id}>{role.name}</SelectItem>
                                    ))}
                                </SelectContent>
                                </Select>
                            </div>
                        </div>
                    </div>

                    <DialogFooter className="dialog-footer">
                        <Button variant="outline" onClick={() => setIsEmployeeDialogOpen(false)}>
                            Cancelar
                        </Button>
                        <Button onClick={editingEmployee ? handleEditEmployee : handleAddEmployee}>
                            {editingEmployee ? "Guardar cambios" : "Agregar empleado"}
                        </Button>
                    </DialogFooter>
                </DialogContent>
            </Dialog>
        </div>
    )
}
