"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { ArrowRightLeft, BanknoteIcon, Calendar, CreditCard, LogOut, Menu, PiggyBank, Receipt, User, Wallet, X, } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet"
import { toast, Toaster } from "sonner"
import "./dashboardStyle.css"

import { useAuth } from '@/context/AuthContext'

function DashboardClient() {

  const navigate = useNavigate()
  const { user, logout } = useAuth()
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false)

  // Manejo de simulacion de salir de sesion
  const handleLogout = () => 
  {
    setIsMobileMenuOpen(false)

    logout() // Limpia el usuario del contexto y del sessionStorage

    // Mostrar notificación
    toast.success("Cerrando sesión")

    setTimeout(() => {
      navigate("/client_login")
    }, 1500)
  }

  return (
    <div className="dashboard-container">
      
      {/* Barra de navegación */}
      <nav className="navbar">

        {/* Titulo de la barra de navegacion */}
        <div className="navbar-content">
          
          {/* Encabezado */}
          <div className="logo-container">
            <BanknoteIcon className="logo-icon" />
            <span className="logo-text">TecBank</span>
          </div>

          {/* Menú desplegable de usuario para salir de la sesion */}
          <div className="user-menu">

            {/* Menú desplegable de usuario */}
            <DropdownMenu>

              {/* Icono de usuario */}
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="icon" className="user-button">
                  <User className="user-icon" />
                  <span className="sr-only">Menú de usuario</span>
                </Button>
              </DropdownMenuTrigger>

              {/* Boton del icono de usuario */}
              <DropdownMenuContent align="end">
                <DropdownMenuItem className="logout-item" onClick={handleLogout}>
                  <LogOut className="logout-icon" />
                  <span className="logout-text">Cerrar sesión</span>
                </DropdownMenuItem>
              </DropdownMenuContent>

            </DropdownMenu>

          </div>

        </div>

        <Toaster position="bottom-right" />
        
      </nav>

      {/* Contenido Principal */}
      <main className="main-content">
        <div className="welcome-section">
          <h1 className="welcome-title">Bienvenido, {user.name}  {user.last_name1}</h1>
          <p className="welcome-description">Seleccione una opción para gestionar sus servicios bancarios.</p>
        </div>

        {/* Tarjetas de Opciones */}
        <div className="cards-grid-dashboard">

          {/* Tarjeta de Cuentas */}
          <Card className="option-card-dashboard">

            {/* Encabezado */}
            <CardHeader className="card-header-dashboard card-header-accounts">
              <div className="card-title-container">
                <Wallet className="card-icon-accounts" />
                <CardTitle className="card-title">Cuentas</CardTitle>
              </div>
              <CardDescription className="card-description">Gestione sus cuentas bancarias</CardDescription>
            </CardHeader>
            
            {/* Contenido */}
            <CardContent className="card-content-dashboard">

              {/* Opcion 1*/}
              <div className="option-link" onClick={() => navigate("/accounts/transactions")}>
                <Receipt className="option-icon-accounts" />
                <div className="option-text">
                  <h3 className="option-title">Ver transacciones de mi cuenta</h3>
                  <p className="option-subtitle">Consulte depósitos, retiros y tarjetas vinculadas</p>
                </div>
              </div>

              {/* Opcion 2*/}
              <div className="option-link" onClick={() => navigate("/accounts/transfer")}>
                <ArrowRightLeft className="option-icon-accounts" />
                <div className="option-text">
                  <h3 className="option-title">Realizar una transferencia</h3>
                  <p className="option-subtitle">Transfiera fondos a otra cuenta</p>
                </div>
              </div>

            </CardContent>

          </Card>

          {/* Tarjeta de Tarjetas */}
          <Card className="option-card-dashboard">
            
            {/* Encabezado */}
            <CardHeader className="card-header-dashboard card-header-cards">
              <div className="card-title-container">
                <CreditCard className="card-icon-cards" />
                <CardTitle className="card-title">Tarjetas</CardTitle>
              </div>
              <CardDescription className="card-description">Gestione sus tarjetas de crédito y débito</CardDescription>
            </CardHeader>

            {/* Contenido*/}
            <CardContent className="card-content-dashboard">
              
              {/* Opcion 1*/}
              <div className="option-link" onClick={() => navigate("/cards/payments")}>
                <CreditCard className="option-icon-cards" />
                <div className="option-text">
                  <h3 className="option-title">Pagos de Tarjeta</h3>
                  <p className="option-subtitle">Realice un pago a una tarjeta de crédito</p>
                </div>
              </div>

            {/* Opcion 2*/}    
              <div className="option-link" onClick={() => navigate("/cards/purchases")}>
                <Calendar className="option-icon-cards" />
                <div className="option-text">
                  <h3 className="option-title">Lista de compras</h3>
                  <p className="option-subtitle">Ver compras por rango de fechas</p>
                </div>
              </div>

            </CardContent>

          </Card>

          {/* Tarjeta de Préstamos */}
          <Card className="option-card-dashboard">
            
            {/* Encabezado */}
            <CardHeader className="card-header-dashboard card-header-loans">
              <div className="card-title-container">
                <PiggyBank className="card-icon-loans" />
                <CardTitle className="card-title">Préstamos</CardTitle>
              </div>
              <CardDescription className="card-description">Gestione sus préstamos y pagos</CardDescription>
            </CardHeader>

            {/* Contenido */}
            <CardContent className="card-content-dashboard">
              <div className="option-link" onClick={() => navigate("/loans/payments")}>
                <BanknoteIcon className="option-icon-loans" />
                <div className="option-text">
                  <h3 className="option-title">Pagos de Préstamos</h3>
                  <p className="option-subtitle">Realice pagos regulares o extraordinarios de préstamos</p>
                </div>
              </div>
            </CardContent>
            
          </Card>

        </div>

      </main>
      
    </div>
  )
}
export default DashboardClient