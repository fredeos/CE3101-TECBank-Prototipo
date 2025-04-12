import './App.css'

import LoginAdmin from './pages/admin/loginAdmin/login_admin'
import DashBoardAdmin from './pages/admin/dashboardAdmin/dashboard_admin'
import GestionRolesAdmin from './pages/admin/gestionRoles/gestion_roles'
import LoginClient from './pages/client/loginClient/loginView'
import DashBoardClient from './pages/client/dashboardClient/dashboardView'

import {BrowserRouter, Routes, Route } from 'react-router-dom';

function App(){
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/loginAdmin' element={<LoginAdmin/>}/>
        <Route path='/adminDashboard' element={<DashBoardAdmin/>}/>
        <Route path='/gestion-roles' element={<GestionRolesAdmin/>} />
        <Route path='/loginClient' element={<LoginClient/>}/>
        <Route path='/dashboardClient' element={<DashBoardClient/>}/>

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