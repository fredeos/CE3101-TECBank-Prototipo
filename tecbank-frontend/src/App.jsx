 // CLiente con los valores mockeados (valores prestablecidos)

import './App.css'

import LoginAdmin from './pages/admin/loginAdmin/login_admin'
import DashBoardAdmin from './pages/admin/dashboardAdmin/dashboard_admin'
import GestionRolesAdmin from './pages/admin/gestionRoles/gestion_roles'
import GestionClientesAdmin from './pages/admin/gestionClientes/gestion_clientes'
import GestionCuentasAdmin from './pages/admin/gestionCuentas/gestion_cuentas'
import GestionTarjetasAdmin from './pages/admin/gestionTarjetas/gestion_tarjetas'
import GestionEmpleadosAdmin from './pages/admin/gestionEmpleados/gestion_empleados'

import LoginClient from './mocks/clientMocks/clientViews/loginView/loginClient'
import DashboardClient from './mocks/clientMocks/clientViews/dashboardView/dashboardClient'
import TransactionsClient from './mocks/clientMocks/clientViews/transactionsView/transactionsClient'
import TransferClient from './mocks/clientMocks/clientViews/transferView/transferClient'
import CardPaymentsClient from './mocks/clientMocks/clientViews/CardPaymentsView/cardsPaymentsClient'
import PurchasesClient from './mocks/clientMocks/clientViews/PurchasesView/purchasesClient'
import LoansClient from './mocks/clientMocks/clientViews/loansPaymentView/loansPaymentClient'

import { BrowserRouter, Routes, Route } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        
        <Route path='/loginAdmin' element={<LoginAdmin/>}/>
        <Route path='/adminDashboard' element={<DashBoardAdmin/>}/>
        <Route path='/gestion-roles' element={<GestionRolesAdmin/>} />
        <Route path='/gestion-clientes' element={<GestionClientesAdmin/>} />
        <Route path='/gestion-cuentas-admin' element={<GestionCuentasAdmin/>}/>
        <Route path='/gestion-tarjetas-admin' element={<GestionTarjetasAdmin/>}/>
        <Route path='/gestion-empleados-admin' element={<GestionEmpleadosAdmin/>}/>

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

export default App




/* // Cliente conectado al backend con los POST

import './App.css'
import { AuthProvider } from './context/AuthContext'

import LoginAdmin from './pages/admin/loginAdmin/login_admin'
import DashBoardAdmin from './pages/admin/dashboardAdmin/dashboard_admin'
import GestionRolesAdmin from './pages/admin/gestionRoles/gestion_roles'
import GestionClientesAdmin from './pages/admin/gestionClientes/gestion_clientes'
import GestionCuentasAdmin from './pages/admin/gestionCuentas/gestion_cuentas'
import GestionTarjetasAdmin from './pages/admin/gestionTarjetas/gestion_tarjetas'
import GestionEmpleadosAdmin from './pages/admin/gestionEmpleados/gestion_empleados'

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
          <Route path='/loginAdmin' element={<LoginAdmin/>}/>
          <Route path='/adminDashboard' element={<DashBoardAdmin/>}/>
          <Route path='/gestion-roles' element={<GestionRolesAdmin/>} />
          <Route path='/gestion-clientes' element={<GestionClientesAdmin/>} />
          <Route path='/gestion-cuentas-admin' element={<GestionCuentasAdmin/>}/>
          <Route path='/gestion-tarjetas-admin' element={<GestionTarjetasAdmin/>}/>
          <Route path='/gestion-empleados-admin' element={<GestionEmpleadosAdmin/>}/>
        </Routes>
        <AuthProvider>
          <Routes>
            <Route path="/client_login" element={<LoginClient/>}/>
            <Route path="/client_dashboard" element={<DashboardClient/>}/>
            <Route path="/accounts/transactions" element={<TransactionsClient/>}/>
            <Route path="/accounts/transfer" element={<TransferClient/>}/>
            <Route path="/cards/payments" element={<CardPaymentsClient/>}/>
            <Route path="/cards/purchases" element={<PurchasesClient/>}/>
            <Route path="/loans/payments" element={<LoansClient/>}/>
          </Routes>
        </AuthProvider>
      </BrowserRouter>
  );
}

export default App */
