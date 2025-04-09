import { useState } from "react";
import '../CSS/dashboard_admin.css'; // Archivo CSS para estilos

export default function AdminDashboard() {
    const [sidebarOpen, setSidebarOpen] = useState(true);

    return (
    <div className="admin-dashboard">
      {/* Sidebar */}
      <aside className={`sidebar ${sidebarOpen ? "open" : "closed"}`}>
        <div className="sidebar-header">
          <div className="logo">
            <i className="bi bi-shield-lock"></i>
            {sidebarOpen && <span>BankAdmin</span>}
          </div>
          <button 
            className="toggle-btn"
            onClick={() => setSidebarOpen(!sidebarOpen)}
          >
            <i className="bi bi-layers"></i>
          </button>
        </div>
        <nav className="sidebar-nav">
          <ul>
            <li>
              <a href="#" className="active">
                <i className="bi bi-speedometer2"></i>
                {sidebarOpen && <span>Dashboard</span>}
              </a>
            </li>
            <li>
              <a href="#">
                <i className="bi bi-people"></i>
                {sidebarOpen && <span>Customers</span>}
              </a>
            </li>
            <li>
              <a href="#">
                <i className="bi bi-credit-card"></i>
                {sidebarOpen && <span>Accounts</span>}
              </a>
            </li>
            <li>
              <a href="#">
                <i className="bi bi-cash-stack"></i>
                {sidebarOpen && <span>Transactions</span>}
              </a>
            </li>
            <li>
              <a href="#">
                <i className="bi bi-person-plus"></i>
                {sidebarOpen && <span>User Management</span>}
              </a>
            </li>
            <li>
              <a href="#">
                <i className="bi bi-bar-chart-line"></i>
                {sidebarOpen && <span>Reports</span>}
              </a>
            </li>
            <li>
              <a href="#">
                <i className="bi bi-gear"></i>
                {sidebarOpen && <span>Settings</span>}
              </a>
            </li>
          </ul>
        </nav>
        <div className="sidebar-footer">
          <div className="user-profile">
            <div className="avatar">AD</div>
            {sidebarOpen && (
              <div className="user-info">
                <p>Admin User</p>
                <small>admin@bankname.com</small>
              </div>
            )}
          </div>
        </div>
      </aside>

      {/* Main Content */}
      <main className="main-content">
        {/* Header */}
        <header className="main-header">
          <div className="search-container">
            <i className="bi bi-search"></i>
            <input type="search" placeholder="Search..." />
          </div>
          <div className="header-actions">
            <button className="notification-btn">
              <i className="bi bi-bell"></i>
              <span className="badge">3</span>
            </button>
            <button className="help-btn">
              <i className="bi bi-question-circle"></i>
            </button>
            <div className="user-dropdown">
              <button className="avatar-btn">AD</button>
              <div className="dropdown-menu">
                <div className="dropdown-header">My Account</div>
                <div className="dropdown-divider"></div>
                <a href="#" className="dropdown-item">Profile</a>
                <a href="#" className="dropdown-item">Settings</a>
                <div className="dropdown-divider"></div>
                <a href="#" className="dropdown-item">Logout</a>
              </div>
            </div>
          </div>
        </header>

        {/* Dashboard Content */}
        <div className="dashboard-content">
          <div className="content-header">
            <h1>Dashboard</h1>
            <button className="export-btn">
              <i className="bi bi-download"></i> Export Report
            </button>
          </div>

          <div className="tabs-container">
            <div className="tabs-header">
              <button className="tab active">Overview</button>
              <button className="tab">Analytics</button>
              <button className="tab">Reports</button>
              <button className="tab">Notifications</button>
            </div>
            
            <div className="tab-content active">
              {/* Stats Cards */}
              <div className="stats-grid">
                <div className="stat-card">
                  <div className="card-header">
                    <span>Total Customers</span>
                    <i className="bi bi-people"></i>
                  </div>
                  <div className="card-body">
                    <h3>12,548</h3>
                    <small className="text-success">+2.5% from last month</small>
                  </div>
                </div>
                <div className="stat-card">
                  <div className="card-header">
                    <span>Active Accounts</span>
                    <i className="bi bi-credit-card"></i>
                  </div>
                  <div className="card-body">
                    <h3>18,325</h3>
                    <small className="text-success">+1.8% from last month</small>
                  </div>
                </div>
                <div className="stat-card">
                  <div className="card-header">
                    <span>Daily Transactions</span>
                    <i className="bi bi-bar-chart-line"></i>
                  </div>
                  <div className="card-body">
                    <h3>2,345</h3>
                    <small className="text-danger">-0.7% from yesterday</small>
                  </div>
                </div>
                <div className="stat-card">
                  <div className="card-header">
                    <span>Revenue</span>
                    <i className="bi bi-currency-dollar"></i>
                  </div>
                  <div className="card-body">
                    <h3>$458,623</h3>
                    <small className="text-success">+4.3% from last month</small>
                  </div>
                </div>
              </div>

              {/* Charts */}
              <div className="charts-grid">
                <div className="chart-card">
                  <div className="card-header">
                    <h4>Transaction Overview</h4>
                    <small>Daily transaction volume for the past 30 days</small>
                  </div>
                  <div className="chart-placeholder">
                    <i className="bi bi-bar-chart"></i>
                    <span>Transaction Chart</span>
                  </div>
                </div>
                <div className="chart-card">
                  <div className="card-header">
                    <h4>Account Distribution</h4>
                    <small>Distribution of account types</small>
                  </div>
                  <div className="chart-placeholder">
                    <i className="bi bi-pie-chart"></i>
                    <span>Account Chart</span>
                  </div>
                </div>
              </div>

              {/* Recent Activity */}
              <div className="activity-card">
                <div className="card-header">
                  <h4>Recent Activity</h4>
                  <small>Latest system activities and alerts</small>
                </div>
                <div className="activity-list">
                  {[1, 2, 3, 4, 5].map((item) => (
                    <div key={item} className="activity-item">
                      <div className="activity-icon">
                        <i className="bi bi-person-plus"></i>
                      </div>
                      <div className="activity-details">
                        <div className="activity-title">
                          <span>New customer account created</span>
                          <small className="badge">Account</small>
                        </div>
                        <p>John Doe created a new checking account</p>
                      </div>
                      <div className="activity-time">2 hours ago</div>
                    </div>
                  ))}
                </div>
                <div className="card-footer">
                  <button className="view-all-btn">View All Activity</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>

      {/* Bootstrap Icons CDN (opcional, puedes reemplazar con SVG o fuentes alternativas) */}
      <link 
        rel="stylesheet" 
        href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" 
      />
    </div>
  );
}