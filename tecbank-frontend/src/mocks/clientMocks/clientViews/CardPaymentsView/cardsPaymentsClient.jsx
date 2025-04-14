"use client"

import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { ArrowLeft, Info, CheckCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import "./CPStyle.css"

import { sourceAccounts } from "@/mocks/clientMocks/clientAccounts"
import { cards } from "@/mocks/clientMocks/clientCards"

function CardPayments() {
    const navigate = useNavigate()
    const currentDate = new Date().toLocaleDateString()

    // Estado para tarjeta seleccionada y detalles de pago
    const [selectedCardId, setSelectedCardId] = useState("")
    const [paymentAmount, setPaymentAmount] = useState("")
    const [paymentType, setPaymentType] = useState("minimum")
    const [paymentAccountId, setPaymentAccountId] = useState("")
    const [isRecurring, setIsRecurring] = useState(false)
    const [isConfirmStep, setIsConfirmStep] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const [isSuccess, setIsSuccess] = useState(false)

    // Obtener detalles de la tarjeta y cuenta seleccionadas
    const selectedCard = cards.find((card) => card.card_num.toString() === selectedCardId)
    const selectedAccount = sourceAccounts.find((account) => account.id === paymentAccountId)
    // Obtener la cuenta asociada y su moneda
    const selectedAccountDetails = sourceAccounts.find(account => account.id === selectedCard?.account_id);
    const currencyId = selectedAccountDetails?.currency_id || 1;

    // Determinar el tipo de tarjeta
    const getCardType = (type) => {
        return type === 1 ? "Débito" : "Crédito"
    }

    // Formatear número de tarjeta para mostrar
    const formatCardNumber = (num) => {
        const str = num.toString()
        return `${str.slice()}`
    }

    // Manejar selección de tarjeta
    const handleCardSelect = (cardId) => {
        setSelectedCardId(cardId)
        setPaymentType("minimum")
        setIsConfirmStep(false)
        setIsSuccess(false)
    }

    // Manejo de cambio de moneda
    const getCurrencySymbol = (currencyId) => {
        const currencySymbols = {
            1: '$', // Dólar
            2: '€', // Euro
            3: '₡'  // Colón costarricense
        };
        return currencySymbols[currencyId] || '$';
    };

    // Manejar envío del formulario
    const handleSubmit = (e) => {
        e.preventDefault()

        // Validar formulario
        if (!selectedCardId || !paymentAmount || !paymentAccountId) {
            toast.error("Información Faltante", {
                description: "Por favor complete todos los campos requeridos para continuar.",
            })
            return
        }

        if (isNaN(paymentAmount) || Number.parseFloat(paymentAmount) <= 0) {
            toast.error("Monto Inválido", {
                description: "Por favor ingrese un monto de pago válido.",
            })
            return
        }

        // Validación específica para tarjetas de crédito (type: 1)
        if (selectedCard.type === 2) {
            if (Number.parseFloat(paymentAmount) > Math.abs(selectedCard.balance)) {
                toast.error("Pago Demasiado Grande", {
                    description: "El monto del pago no puede exceder el saldo pendiente.",
                })
                return
            }
        }

        if (selectedAccount && (Number.parseFloat(paymentAmount) > selectedAccount.balance)) {
            toast.error("Fondos Insuficientes", {
                description: "El saldo de su cuenta es insuficiente para este pago.",
            })
            return
        }

        if (!isConfirmStep) {
            setIsConfirmStep(true)
            return
        }

        setIsLoading(true)

        setTimeout(() => {
            setIsLoading(false)
            setIsSuccess(true)
            setIsConfirmStep(false)

            toast.success("Pago Exitoso", {
                description: `El pago de ${getCurrencySymbol(currencyId) + ' ' + paymentAmount} ha sido procesado exitosamente.`,
            })
        }, 1500)
    }

    const handleReset = () => {
        setSelectedCardId("")
        setPaymentAmount("")
        setPaymentType("minimum")
        setPaymentAccountId("")
        setIsRecurring(false)
        setIsSuccess(false)
    }

    return (
        <div className="card-payments-container">
            <div className="card-container">
                {/* Botón de regreso */}
                <div className="back-link-container">
                    <Button variant="ghost" className="back-link-client" onClick={() => navigate("/client_dashboard")}>
                        <ArrowLeft className="back-icon" />
                        <span>Volver al Panel</span>
                    </Button>
                </div>

                <div className="card-header">
                    <h1 className="page-title">Pagos de Tarjeta</h1>
                    <p className="page-description">Realice un pago a su tarjeta</p>
                </div>

                {isSuccess ? (
                    <div className="success-card">
                        <div className="success-icon-container">
                            <CheckCircle className="success-icon-check" />
                        </div>
                        <h2 className="success-title">¡Pago Exitoso!</h2>
                        <p className="success-message">
                            Su pago de ${Number.parseFloat(paymentAmount).toFixed(2)} a {formatCardNumber(selectedCard.card_num)} ha sido procesado
                            exitosamente.
                        </p>
                        <div className="success-details">
                            <div className="success-detail-row">
                                <span className="success-label">Tarjeta</span>
                                <span className="success-value">{formatCardNumber(selectedCard.card_num)}</span>
                            </div>
                            <div className="success-detail-row">
                                <span className="success-label">Tipo</span>
                                <span className="success-value">{getCardType(selectedCard.type)}</span>
                            </div>
                            <div className="success-detail-row">
                                <span className="success-label">Monto</span>
                                <span className="success-value">
                                    {getCurrencySymbol(currencyId) + ' ' + paymentAmount}
                                </span>
                            </div>
                            <div className="success-detail-row">
                                <span className="success-label">Desde Cuenta</span>
                                <span className="success-value">{sourceAccounts.description}</span>
                            </div>
                            <div className="success-detail-row">
                                <span className="success-label">Fecha</span>
                                <span className="success-value">{currentDate}</span>
                            </div>
                        </div>
                        <Button onClick={handleReset} className="another-payment-button">
                            Realizar Otro Pago
                        </Button>
                    </div>
                ) : (
                    <div className="card-payment-card">
                        <div className="card-payment-header">
                            <h2 className="card-payment-title">{isConfirmStep ? "Confirmar Pago" : "Realizar un Pago"}</h2>
                            <p className="card-payment-description">
                                {isConfirmStep
                                    ? "Revise y confirme los detalles de su pago"
                                    : "Seleccione una tarjeta e ingrese los detalles del pago"}
                            </p>
                        </div>

                        <form onSubmit={handleSubmit}>
                            {!isConfirmStep ? (
                                <div className="form-content">
                                    {/* Selección de Tarjeta */}
                                    <div className="form-group-client">
                                        <Label htmlFor="cardSelect" className="form-label">
                                            Seleccionar Tarjeta
                                        </Label>
                                        <div className="card-select">
                                            <Select value={selectedCardId} onValueChange={handleCardSelect}>
                                                <SelectTrigger id="cardSelect">
                                                    <SelectValue placeholder="Elija una tarjeta" />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    {cards
                                                        .filter(card => card.type === 2)  // Filtra tarjetas de débito
                                                        .map((card) => (
                                                            <SelectItem key={card.card_num} value={card.card_num.toString()}>
                                                                {getCardType(card.type)} - {formatCardNumber(card.card_num)}
                                                            </SelectItem>
                                                        ))}
                                                </SelectContent>
                                            </Select>
                                        </div>
                                    </div>

                                    {/* Detalles de la Tarjeta Seleccionada */}
                                    {selectedCard && (
                                        <div className={`credit-card-display`}>
                                            <div className="credit-card-content">
                                                <div className="credit-card-issuer">{getCardType(selectedCard.type)}</div>
                                                <div className="credit-card-number">
                                                    <span>Número de tarjeta: </span>
                                                    {formatCardNumber(selectedCard.card_num)}
                                                </div>
                                            </div>
                                            <div className="credit-card-balance-section">
                                                <div className="credit-card-balance-row">
                                                    <span>Cuenta asociada</span>
                                                    <span className="balance-amount">{selectedCard.account_id}</span>
                                                </div>
                                                <div className="credit-card-balance-row">
                                                    <span>Saldo pendiente</span>
                                                    <span className="balance-amount">
                                                        {getCurrencySymbol(currencyId) + ' ' + selectedCard.balance}
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    )}

                                    {selectedCardId && (
                                        <>
                                            {/* Monto de Pago */}
                                            <div className="form-group-client">
                                                <Label htmlFor="paymentAmount" className="form-label">
                                                    Ingresar Monto
                                                </Label>
                                                <div className="input-container">
                                                    <span className="currency-symbol">
                                                        {getCurrencySymbol(currencyId)}
                                                    </span>
                                                    <Input
                                                        id="paymentAmount"
                                                        className="payment-amount-input"
                                                        onChange={(e) => setPaymentAmount(e.target.value)}
                                                        placeholder="0.00"
                                                    />
                                                </div>
                                            </div>

                                            {/* Cuenta de Pago */}
                                            <div className="form-group-client">
                                                <Label htmlFor="paymentAccount" className="form-label">
                                                    Pagar Desde
                                                </Label>
                                                <Select value={paymentAccountId} onValueChange={setPaymentAccountId}>
                                                    <SelectTrigger id="paymentAccount" className="card-select">
                                                        <SelectValue placeholder="Seleccione cuenta" />
                                                    </SelectTrigger>
                                                    <SelectContent>
                                                        {sourceAccounts
                                                            .filter(account => account.id !== selectedCard?.account_id)
                                                            .map(account => (
                                                                <SelectItem key={account.id} value={account.id}>
                                                                    <div className="account-option">
                                                                        <span>{account.description + ': ' + (account.id)}</span>
                                                                        <span className="account-balance">
                                                                            {getCurrencySymbol(account.currency_id)}{account.balance.toFixed(2)}
                                                                        </span>
                                                                    </div>
                                                                </SelectItem>
                                                            ))
                                                        }
                                                    </SelectContent>
                                                </Select>
                                            </div>

                                            {/* Fecha de Pago */}
                                            <div className="form-group-client">
                                                <Label className="form-label">Fecha de Pago</Label>
                                                <div className="payment-date">{currentDate}</div>
                                            </div>
                                        </>
                                    )}
                                </div>
                            ) : (
                                /* Paso de Confirmación */
                                <div className="confirm-container">
                                    <div className="confirm-section">
                                        <div className="confirm-row">
                                            <span className="confirm-label">Tarjeta</span>
                                            <span className="confirm-value">{getCardType(selectedCard.type)}</span>
                                        </div>
                                        <div className="confirm-row">
                                            <span className="confirm-label">Número de Tarjeta</span>
                                            <span className="confirm-value">{formatCardNumber(selectedCard.card_num)}</span>
                                        </div>
                                        <div className="confirm-row">
                                            <span className="confirm-label">Monto de Pago</span>
                                            <span className="confirm-amount-pay">
                                                {getCurrencySymbol(currencyId) + ' ' + paymentAmount}
                                            </span>
                                        </div>
                                        <div className="confirm-row">
                                            <span className="confirm-label">Desde Cuenta</span>
                                            <span className="confirm-value">{selectedAccount.description}</span>
                                        </div>
                                        <div className="confirm-row">
                                            <span className="confirm-label">Fecha de Pago</span>
                                            <span className="confirm-value">{currentDate}</span>
                                        </div>
                                    </div>

                                    <div className="info-box">
                                        <div className="info-content">
                                            <Info className="info-icon" />
                                            <p className="info-text">
                                                Al confirmar, el monto de pago se deducirá de su cuenta seleccionada y se aplicará a su tarjeta.
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            )}

                            <div className="form-actions">
                                {isConfirmStep && (
                                    <button type="button" className="back-button" onClick={() => setIsConfirmStep(false)}>
                                        Atrás
                                    </button>
                                )}
                                <button
                                    type="submit"
                                    className={`submit-button ${isConfirmStep ? "" : "full-width"}`}
                                    disabled={!selectedCardId || isLoading}
                                >
                                    {isLoading ? "Procesando..." : isConfirmStep ? "Confirmar Pago" : "Continuar"}
                                </button>
                            </div>
                        </form>
                    </div>
                )}
            </div>
            <Toaster position="bottom-right" />
        </div>
    )
}

export default CardPayments