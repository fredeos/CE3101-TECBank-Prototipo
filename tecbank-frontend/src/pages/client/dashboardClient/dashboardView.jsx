import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { ArrowRightLeft, BanknoteIcon, Calendar, CreditCard, LogOut, Menu, PiggyBank, Receipt, User, Wallet, X } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet"
import "./dashboardView.css"

export default function DashBoardView() {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false)
  const navigate = useNavigate()

  // Funciones de navegación
  const goToAccountTransactions = () => navigate("/accounts/transactions")
  const goToTransfer = () => navigate("/accounts/transfer")
  const goToCardPayments = () => navigate("/cards/payments")
  const goToPurchases = () => navigate("/cards/purchases")
  const goToLoanPayments = () => navigate("/loans/payments")

  const hardcodeUser = "developer"

  return (
    <div className="dashboard-container">
      {/* Navbar */}
      <nav className="dashboard-nav">
        <div className="nav-container">
          <div className="nav-content">
            <div className="nav-logo">
              <BanknoteIcon className="logo-icon" />
              <span className="logo-text">TecBank</span>
            </div>

            {/* User Dropdown Menu */}
            <div className="user-menu">
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="ghost" size="icon" className="user-button">
                    <User className="user-icon" />
                    <span className="sr-only">User menu</span>
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                  <DropdownMenuItem className="dropdown-item">
                    <LogOut className="dropdown-icon" />
                    <span>Log out</span>
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>

              {/* Mobile menu button */}
              <div className="mobile-menu-button">
                <Sheet open={isMobileMenuOpen} onOpenChange={setIsMobileMenuOpen}>
                  <SheetTrigger asChild>
                    <Button variant="ghost" size="icon">
                      {isMobileMenuOpen ? <X className="mobile-menu-icon" /> : <Menu className="mobile-menu-icon" />}
                    </Button>
                  </SheetTrigger>
                  <SheetContent side="right" className="mobile-menu-content">
                    <div className="mobile-menu-items">
                      <div className="mobile-user-info">
                        <User className="mobile-user-icon" />
                        <span className="mobile-user-name">John Doe</span>
                      </div>
                      <hr className="mobile-menu-divider" />
                      <button
                        className="mobile-logout-button"
                        onClick={() => setIsMobileMenuOpen(false)}
                      >
                        <LogOut className="mobile-logout-icon" />
                        <span>Log out</span>
                      </button>
                    </div>
                  </SheetContent>
                </Sheet>
              </div>
            </div>
          </div>
        </div>
      </nav>

      {/* Main Content */}
      <main className="main-content">
        <div className="welcome-section">
          <h1 className="welcome-title">Welcome, {hardcodeUser}</h1>
          <p className="welcome-subtitle">Select an option below to manage your banking services.</p>
        </div>

        {/* Option Cards */}
        <div className="cards-grid">
          {/* Accounts Card */}
          <Card className="account-card">
            <CardHeader className="card-header bg-blue">
              <div className="card-title-container">
                <Wallet className="card-icon" />
                <CardTitle>Accounts</CardTitle>
              </div>
              <CardDescription>Manage your bank accounts</CardDescription>
            </CardHeader>
            <CardContent className="card-options">
              <button 
                onClick={goToAccountTransactions}
                className="card-button"
              >
                <Receipt className="option-icon text-blue" />
                <div className="option-text">
                  <h3 className="option-title">View my account transactions</h3>
                  <p className="option-description">Check deposits, withdrawals and linked cards</p>
                </div>
              </button>

              <button 
                onClick={goToTransfer}
                className="card-button"
              >
                <ArrowRightLeft className="option-icon text-blue" />
                <div className="option-text">
                  <h3 className="option-title">Make a money transfer</h3>
                  <p className="option-description">Transfer funds to another account</p>
                </div>
              </button>
            </CardContent>
          </Card>

          {/* Cards Card */}
          <Card className="card-card">
            <CardHeader className="card-header bg-purple">
              <div className="card-title-container">
                <CreditCard className="card-icon" />
                <CardTitle>Cards</CardTitle>
              </div>
              <CardDescription>Manage your credit and debit cards</CardDescription>
            </CardHeader>
            <CardContent className="card-options">
              <button 
                onClick={goToCardPayments}
                className="card-button"
              >
                <CreditCard className="option-icon text-purple" />
                <div className="option-text">
                  <h3 className="option-title">Card Payments</h3>
                  <p className="option-description">Make a payment to a credit card</p>
                </div>
              </button>

              <button 
                onClick={goToPurchases}
                className="card-button"
              >
                <Calendar className="option-icon text-purple" />
                <div className="option-text">
                  <h3 className="option-title">List of purchases</h3>
                  <p className="option-description">View purchases by date range</p>
                </div>
              </button>
            </CardContent>
          </Card>

          {/* Loans Card */}
          <Card className="loan-card">
            <CardHeader className="card-header bg-green">
              <div className="card-title-container">
                <PiggyBank className="card-icon" />
                <CardTitle>Loans</CardTitle>
              </div>
              <CardDescription>Manage your loans and payments</CardDescription>
            </CardHeader>
            <CardContent className="card-options">
              <button 
                onClick={goToLoanPayments}
                className="card-button"
              >
                <BanknoteIcon className="option-icon text-green" />
                <div className="option-text">
                  <h3 className="option-title">Loan Payments</h3>
                  <p className="option-description">Make regular or extraordinary loan payments</p>
                </div>
              </button>
            </CardContent>
          </Card>
        </div>
      </main>
    </div>
  )
}