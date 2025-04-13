import './App.css'

import LoginAdmin from './pages/admin/loginAdmin/login_admin'
import DashBoardAdmin from './pages/admin/dashboardAdmin/dashboard_admin'
import GestionRolesAdmin from './pages/admin/gestionRoles/gestion_roles'
import GestionClientesAdmin from './pages/admin/gestionClientes/gestion_clientes'
/* + ========================= Cliente ========================= + */
import LoginClient from './pages/client/loginView/loginClient'
import DashboardClient from './pages/client/dashboardView/dashboardClient'
import TransactionsClient from './pages/client/transactionsView/transactionsClient'
import TransferClient from './pages/client/transferView/transferClient'
import CardPaymentsClient from './pages/client/CardPaymentsView/cardsPaymentsClient'
import PurchasesClient from './pages/client/PurchasesView/purchasesClient'
import LoansClient from './pages/client/loansPaymentView/loansPaymentClient'


import { BrowserRouter, Routes, Route } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* + ========================= Admin ========================= + */}
        <Route path='/loginAdmin' element={<LoginAdmin/>}/>
        <Route path='/adminDashboard' element={<DashBoardAdmin/>}/>
        <Route path='/gestion-roles' element={<GestionRolesAdmin/>} />
        <Route path='/gestion-clientes' element={<GestionClientesAdmin/>} />
        {/* + ========================= Cliente ========================= + */}
        <Route path="/client_login" element={<LoginClient/>}/>
        <Route path="/client_dashboard" element={<DashboardClient/>}/>
        <Route path="/accounts/transactions" element={<TransactionsClient/>}/>
        <Route path="/accounts/transfer" element={<TransferClient/>}/>
        <Route path="/cards/payments" element={<CardPaymentsClient/>}/>
        <Route path="/cards/purchases" element={<PurchasesClient/>}/>
        <Route path="/loans/payments" element={<LoansClient/>}/>
      
      </Routes>
    </BrowserRouter>
  );
}

/*
<Route path="/" element={<Home/>}/>
import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'


import { Button } from "@/components/ui/button"
 
function App() {
  return (
    <div className="flex flex-col items-center justify-center min-h-svh">
      <Button>Click me</Button>
    </div>
  )

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.jsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

import { Button } from "@/components/ui/button"

function App() {
  return (
    <div className="flex flex-col items-center justify-center min-h-svh">
      <Button>Click me</Button>
    </div>
  )
}
*/

export default App