"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { ArrowLeft, ArrowRight, Info } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"
import "./transferStyle.css"

// Datos de ejemplo para cuentas de origen
const sourceAccounts = [
  {
    id: "1",
    name: "Cuenta Corriente Principal",
    number: "**** 4567",
    balance: 3250.75,
    currency: "USD",
  },
  {
    id: "2",
    name: "Cuenta de Ahorros",
    number: "**** 8901",
    balance: 12500.5,
    currency: "USD",
  },
]

function MoneyTransfer() {

  const navigate = useNavigate()

  // Estado del formulario
  const [step, setStep] = useState(1)
  const [sourceAccount, setSourceAccount] = useState(sourceAccounts[0].id)
  const [recipientAccount, setRecipientAccount] = useState("")
  const [amount, setAmount] = useState("")
  const [description, setDescription] = useState("")
  const [scheduledDate, setScheduledDate] = useState("")
  const [isLoading, setIsLoading] = useState(false)

  // Obtener detalles de la cuenta de origen seleccionada
  const selectedAccount = sourceAccounts.find((account) => account.id === sourceAccount)

  // Manejar envío del formulario
  const handleSubmit = (e) => {
    e.preventDefault()

    // Validar formulario
    if (!sourceAccount || !recipientAccount || !amount) {
      toast.error("Información Faltante", {
        description: "Por favor complete todos los campos requeridos para continuar.",
      })
      return
    }

    if (isNaN(amount) || Number.parseFloat(amount) <= 0) {
      toast.error("Monto Inválido", {
        description: "Por favor ingrese un monto de transferencia válido.",
      })
      return
    }

    if (Number.parseFloat(amount) > selectedAccount.balance) {
      toast.error("Fondos Insuficientes", {
        description: "El saldo de su cuenta es insuficiente para esta transferencia.",
      })
      return
    }

    // Si la validación pasa, avanzar al paso de confirmación
    if (step === 1) {
      setStep(2)
      return
    }

    // Procesar transferencia (paso 2)
    setIsLoading(true)

    // Simular llamada a API
    setTimeout(() => {
      setIsLoading(false)
      toast.success("Transferencia Exitosa", {
        description: `${Number.parseFloat(amount).toFixed(2)} ha sido transferido exitosamente.`,
      })

      // Reiniciar formulario
      setStep(1)
      setRecipientAccount("")
      setAmount("")
      setDescription("")
      setScheduledDate("")
    }, 1500)
  }

  return (
    <div className="transfer-container">
      <div className="transfer-content">
        {/* Botón de regreso */}
        <div className="back-link-container">
          <Button variant="ghost" className="back-link-client" onClick={() => navigate("/client_dashboard")}>
              <ArrowLeft className="back-icon" />
              <span>Volver al Panel</span>
          </Button>
        </div>

        <div className="page-header">
          <h1 className="page-title">Transferencia de Dinero</h1>
          <p className="page-description">Transfiera fondos a otra cuenta de forma rápida y segura</p>
        </div>

        <Card className="transfer-card">
          <CardHeader className="card-header">
            <CardTitle className="card-title">
              {step === 1 ? "Detalles de Transferencia" : "Confirmar Transferencia"}
            </CardTitle>
            <CardDescription className="card-description">
              {step === 1
                ? "Ingrese los detalles para su transferencia de dinero"
                : "Por favor revise y confirme los detalles de su transferencia"}
            </CardDescription>
          </CardHeader>
          <CardContent className="card-content">
            <form onSubmit={handleSubmit} className="form-container">
              {step === 1 ? (
                <>
                  {/* Selección de Cuenta de Origen */}
                  <div className="form-group-client">
                    <Label htmlFor="sourceAccount" className="form-label">
                      Desde Cuenta
                    </Label>
                    <Select value={sourceAccount} onValueChange={setSourceAccount}>
                      <SelectTrigger id="sourceAccount" className="form-select">
                        <SelectValue placeholder="Seleccione cuenta de origen" />
                      </SelectTrigger>
                      <SelectContent>
                        {sourceAccounts.map((account) => (
                          <SelectItem key={account.id} value={account.id}>
                            <div className="account-option">
                              <span>{account.name}</span>
                              <span className="account-balance">${account.balance.toFixed(2)}</span>
                            </div>
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>

                  {/* Cuenta Destinataria */}
                  <div className="form-group-client">
                    <Label htmlFor="recipientAccount" className="form-label">
                      Número de Cuenta
                    </Label>
                    <Input
                      id="recipientAccount"
                      value={recipientAccount}
                      onChange={(e) => setRecipientAccount(e.target.value)}
                      placeholder="Ingrese número de cuenta"
                      className="form-input"
                    />
                  </div>

                  {/* Monto y Descripción */}
                  <div className="form-group-client">
                    <Label htmlFor="amount" className="form-label">
                      Monto
                    </Label>
                    <div className="amount-input-container">
                      <span className="currency-symbol">$</span>
                      <Input
                        id="amount"
                        className="form-input amount-input"
                        value={amount}
                        onChange={(e) => setAmount(e.target.value)}
                        placeholder="0.00"
                      />
                    </div>
                  </div>

                  <div className="form-group-client">
                    <Label htmlFor="description" className="form-label">
                      Descripción (Opcional)
                    </Label>
                    <Textarea
                      id="description"
                      value={description}
                      onChange={(e) => setDescription(e.target.value)}
                      placeholder="Añadir una nota o referencia"
                      className="form-textarea"
                    />
                  </div>
                </>
              ) : (
                /* Paso de Confirmación */
                <>
                  <div className="confirm-container">
                    <div className="transfer-flow">
                      <div className="account-details">
                        <p className="account-label">Desde</p>
                        <p className="account-name">{selectedAccount.name}</p>
                        <p className="account-number">{selectedAccount.number}</p>
                      </div>
                      <ArrowRight className="arrow-icon" />
                      <div className="account-details">
                        <p className="account-label">Para</p>
                        <p className="account-name">Cuenta</p>
                        <p className="account-number">{recipientAccount}</p>
                      </div>
                    </div>

                    <div className="transfer-details">
                      <div className="detail-row">
                        <span className="detail-label">Monto</span>
                        <span className="detail-value">${Number.parseFloat(amount).toFixed(2)}</span>
                      </div>
                      <div className="detail-row">
                        <span className="detail-label">Comisión</span>
                        <span className="detail-value-small">$0.00</span>
                      </div>
                      <div className="total-row">
                        <span>Total</span>
                        <span>${Number.parseFloat(amount).toFixed(2)}</span>
                      </div>
                    </div>

                    {description && (
                      <div className="description-section">
                        <p className="description-label">Descripción</p>
                        <p>{description}</p>
                      </div>
                    )}

                    {scheduledDate && (
                      <div className="description-section">
                        <p className="description-label">Fecha de Transferencia</p>
                        <p>{new Date(scheduledDate).toLocaleDateString()}</p>
                      </div>
                    )}
                  </div>

                  <div className="info-box">
                    <Info className="info-icon" />
                    <p className="info-text">
                      Al confirmar esta transferencia, acepta nuestros términos y condiciones para transferencias de
                      dinero.
                    </p>
                  </div>
                </>
              )}

              <div className="form-actions">
                {step === 2 && (
                  <button type="button" className="back-action" onClick={() => setStep(1)}>
                    Atrás
                  </button>
                )}
                <button
                  type="submit"
                  className={`submit-action ${step === 1 ? "full-width" : ""}`}
                  disabled={isLoading}
                >
                  {isLoading ? "Procesando..." : step === 1 ? "Continuar" : "Confirmar Transferencia"}
                </button>
              </div>
            </form>
          </CardContent>
        </Card>
      </div>
      <Toaster position="top-center" />
    </div>
  )
}

export default MoneyTransfer