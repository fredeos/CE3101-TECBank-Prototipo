import { useState } from "react";
import { BarChart3, CreditCard, DollarSign, LayoutDashboard, Settings, Shield, Users, UserPlus } from "lucide-react"
import { Card, CardContent } from "@/components/ui/card"
import './dashboard_admin.css'; // Archivo CSS para estilos
import { Navigate, NavLink } from "react-router-dom";
import { useNavigate } from "react-router-dom"


const AdminDashboard = () => {
  // Admin user info
  const adminUser = {
    name: "Admin User",
    email: "admin@bankname.com",
  }
  const navigate = useNavigate()

  const goToPrueba = () => navigate("/gestion-roles")
  
  // Admin options for the dashboard
  const adminOptions = [
    {
      title: "Gestión de roles",
      icon: UserPlus,
      colorClass: "icon-container-blue",
      NavLink: goToPrueba
    },
    {
      title: "Gestión de clientes",
      icon: Users,
      colorClass: "icon-container-green",
      NavLink: goToPrueba
    },
    {
      title: "Gestión de cuentas",
      icon: CreditCard,
      colorClass: "icon-container-purple",
      NavLink: goToPrueba
    },
    {
      title: "Gestión de tarjetas",
      icon: CreditCard,
      colorClass: "icon-container-amber",
      NavLink: goToPrueba
    },
    {
      title: "Gestión de asesores de crédito",
      icon: UserPlus,
      colorClass: "icon-container-rose",
      NavLink: goToPrueba
    },
    {
      title: "Gestión de préstamos",
      icon: BarChart3,
      colorClass: "icon-container-cyan",
      NavLink: goToPrueba
    },
    {
      title: "Gestión de mora",
      icon: Settings,
      colorClass: "icon-container-slate",
      NavLink: goToPrueba
    }
  ]

  
  return (
    <div className="admin-dashboard">
      {/* Header */}
      <header className="admin-header">
        <div className="header-logo">
          <Shield className="header-logo-icon" />
          <span className="header-title">TecBank</span>
        </div>

        {/* User info in upper right */}
        <div className="user-info">
          <p className="user-name">{adminUser.name}</p>
          <p className="user-email">{adminUser.email}</p>
        </div>
      </header>

      {/* Main Content */}
      <main className="admin-main">
        <div className="main-container">
          <h1 className="dashboard-title">Panel de control de administración</h1>

          <div className="options-grid">
            {adminOptions.map((option) => (
              <Card
                key={option.title}
                className="option-card"
                onClick={option.NavLink}
              >
              {/* onClick={() => console.log(`Clicked on ${option.title}`)} */}
                <CardContent className="card-content">
                  <div className={`icon-container ${option.colorClass}`}>
                    <option.icon className="option-icon" />
                  </div>
                  <h3 className="option-title">{option.title}</h3>
                  {/*<p className="option-description">{option.description}</p>*/}
                </CardContent>
              </Card>
            ))}
          </div>
        </div>
      </main>
    </div>
  )
}

export default AdminDashboard