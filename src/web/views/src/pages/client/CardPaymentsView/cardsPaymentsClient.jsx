"use client"

import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import { toast, Toaster } from "sonner"
import { ArrowLeft, Info, CheckCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useAuth } from "@/context/AuthContext"
import "./cardsPaymentsStyle.css"

function CardPayments() {
    const navigate = useNavigate()
    const { user } = useAuth() // Obtenemos los datos del usuario
    const currentDate = new Date().toLocaleDateString()

    // Estados para datos
    const [cards, setCards] = useState([])
    const [accounts, setAccounts] = useState([])
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState(null)

    // Estados del formulario
    const [selectedCardId, setSelectedCardId] = useState("")
    const [paymentAmount, setPaymentAmount] = useState("")
    const [paymentType, setPaymentType] = useState("minimum")
    const [paymentAccountId, setPaymentAccountId] = useState("")
    const [isRecurring, setIsRecurring] = useState(false)
    const [isConfirmStep, setIsConfirmStep] = useState(false)
    const [isLoadingSubmit, setIsLoadingSubmit] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const [isSuccess, setIsSuccess] = useState(false)

    // Obtiene datos del backend
    useEffect(() => 
    {   
        // Si no hay usuario logueado con ID, no realiza la petición
        if (!user?.id) 
            return

        // Funcion para cargar datos del cliente desde el backend
        const fetchData = async () => 
        {
            try 
            {
                setLoading(true)

                // Obtiene las tarjetas del cliente
                const cardsResponse = await fetch(
                    `http://192.168.100.59:5055/services/client/${user.id}/cards`
                )
                const cardsData = await cardsResponse.json()
                setCards(cardsData) // Guarda tarjetas en el estado

                // Obtiene las cuentas del cliente
                const accountsResponse = await fetch(
                    `http://192.168.100.59:5055/services/client/${user.id}/accounts`
                )
                const accountsData = await accountsResponse.json()
                setAccounts(accountsData) // Guarda cuentas en el estado

            } 
            catch (err) 
            {
                console.error("Error fetching data:", err)
                setError("No se pudieron cargar los datos")
                toast.error("Error", {description: "No se pudieron cargar los datos de tarjetas y cuentas",})
            } 
            finally 
            {
                setLoading(false)
            }
        }

        fetchData()
    }, [user?.id])

    // Obtener detalles de los elementos seleccionados
    const selectedCard = cards.find((card) => card.card_num.toString() === selectedCardId)
    const selectedAccount = accounts.find((account) => account.id === paymentAccountId)
    const selectedCardAccount = accounts.find(account => account.id === selectedCard?.account_id)
    const currencyId = selectedCardAccount?.currency_id || 1 
 
    // Determinar el tipo de tarjeta
    const getCardType = (type) => 
    {
        return type === 2 ? "Débito" : "Crédito" // Tipo 2 = Débito, cualquier otro valor = Crédito
    }

    // Formatear número de tarjeta para mostrar (solo últimos 4 dígitos - OPCIONAL)
    const formatCardNumber = (num) => 
    {
        const str = num.toString()
        return `${str.slice()}`
    }

    // Manejar selección de tarjeta
    const handleCardSelect = (cardId) => 
    {
        setSelectedCardId(cardId)
        setPaymentType("minimum")
        setIsConfirmStep(false)
        setIsSuccess(false)
    }

    // Manejo simbolo moneda
    const getCurrencySymbol = (currencyId) => 
    {
        const currencySymbols = {
            1: '$', // Dólar
            2: '€', // Euro
            3: '₡'  // Colón costarricense
        };
        return currencySymbols[currencyId] || '$';
    };

    // Manejar envío del formulario
    const handleSubmit = async (e) => 
    {
        e.preventDefault()

        // Validación de campos obligatorios
        if (!selectedCardId || !paymentAmount || !paymentAccountId) 
        {
            toast.error("Información Faltante", {description: "Por favor complete todos los campos requeridos para continuar.",})
            return
        }

        // Validación de monto válido
        if (isNaN(paymentAmount) || Number.parseFloat(paymentAmount) <= 0) 
        {
            toast.error("Monto Inválido", {description: "Por favor ingrese un monto de pago válido.",})
            return
        }

        // Validar que el monto no supere el saldo de deuda (solo si la tarjeta es de crédito)
        if (selectedCard?.type === 1 && Number.parseFloat(paymentAmount) > Math.abs(selectedCard.balance)) 
        {
            toast.error("Pago Demasiado Grande", {description: "El monto del pago no puede exceder el saldo pendiente.",})
            return
        }

        // Validar que la cuenta de origen tenga saldo suficiente
        if (selectedAccount && (Number.parseFloat(paymentAmount) > selectedAccount.balance)) 
        {
            toast.error("Fondos Insuficientes", {description: "El saldo de su cuenta es insuficiente para este pago.",})
            return
        }

        // Si aún no se ha confirmado, activa el paso de confirmación
        if (!isConfirmStep) 
        {
            setIsConfirmStep(true)
            return
        }

        setIsLoadingSubmit(true)

        try 
        {
            // Aquí iría la llamada a la API para procesar el pago
            // const response = await fetch(...)

            // Simulación de éxito
            toast.success("Pago Exitoso", {description: `El pago de ${getCurrencySymbol(selectedAccount.currency_id)}${paymentAmount} ha sido procesado exitosamente.`,})
            setIsSuccess(true)
        } 
        catch (error) 
        {
            toast.error("Error en Pago", {description: "Ocurrió un error al procesar el pago. Por favor intente nuevamente.",})
        } 
        finally 
        {
            setIsLoadingSubmit(false)
            setIsConfirmStep(false)
        }
    }

    // Reinicio de los estados del formulario
    const handleReset = () => 
    {
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
                
                {/* Encabezado y descripcion */}
                <div className="card-header">
                    <h1 className="page-title">Pagos de Tarjeta</h1>
                    <p className="page-description">Realice un pago a su tarjeta</p>
                </div>

                {isSuccess ? (

                    /* En caso de que sea exitoso */
                    <div className="success-card">

                        <div className="success-icon-container">
                            <CheckCircle className="success-icon-check" />
                        </div>

                        <h2 className="success-title">¡Pago Exitoso!</h2>
                        
                        {/* Mensaje de exito */}
                        <p className="success-message">
                            Su pago de ${Number.parseFloat(paymentAmount).toFixed(2)} a {formatCardNumber(selectedCard.card_num)} ha sido procesado
                            exitosamente.
                        </p>
                        
                        {/* Detalles de exito */}
                        <div className="success-details">

                            {/* Numero de tarjeta */}
                            <div className="success-detail-row">
                                <span className="success-label">Tarjeta</span>
                                <span className="success-value">{formatCardNumber(selectedCard.card_num)}</span>
                            </div>

                            {/* Tipo de tarjeta */}
                            <div className="success-detail-row">
                                <span className="success-label">Tipo</span>
                                <span className="success-value">{getCardType(selectedCard.type)}</span>
                            </div>

                            {/* Monto */}
                            <div className="success-detail-row">
                                <span className="success-label">Monto</span>
                                <span className="success-value">
                                    {getCurrencySymbol(currencyId) + ' ' + paymentAmount}
                                </span>
                            </div>

                            {/* Cuenta origen */}
                            <div className="success-detail-row">
                                <span className="success-label">Desde Cuenta</span>
                                <span className="success-value">{selectedAccount.description}</span>
                            </div>

                            {/* Fecha */}
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
                        
                        {/* Encabezados de formulario */}
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
                                                        .filter(card => card.type === 1)  // Filtra tarjetas de crédito
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
                                            
                                            {/* Numero de tarjeta */}
                                            <div className="credit-card-content">
                                                <div className="credit-card-issuer">{getCardType(selectedCard.type)}</div>
                                                <div className="credit-card-number">
                                                    <span>Número de tarjeta: </span>
                                                    {formatCardNumber(selectedCard.card_num)}
                                                </div>
                                            </div>

                                            {/* Cuenta asociada */}
                                            <div className="credit-card-balance-section">

                                                {/* Cuenta */}
                                                <div className="credit-card-balance-row">
                                                    <span>Cuenta asociada</span>
                                                    <span className="balance-amount">
                                                        {selectedCardAccount?.description || selectedCard?.account_id}
                                                    </span>
                                                </div>

                                                {/* Saldo pendiente */}
                                                <div className="credit-card-balance-row">
                                                    <span>Saldo pendiente</span>
                                                    <span className="balance-amount">
                                                        {getCurrencySymbol(currencyId)}{Math.abs(selectedCard?.balance || 0).toFixed(2)}
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
                                                        {selectedAccount ? getCurrencySymbol(selectedAccount.currency_id) : '$'}
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
                                                        {/*.filter(account => account.id !== selectedCard?.account_id)*/}
                                                        {accounts
                                                            .map(account => (
                                                                <SelectItem key={account.id} value={account.id}>
                                                                    <div className="account-option">
                                                                        <span>{account.description}</span>
                                                                        <span className="account-balance">
                                                                            {console.log(getCurrencySymbol(account.currency_id))}
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

                                    {/* Resumen de pago */}
                                    <div className="confirm-section">
                                        
                                        {/* Tipo de tarjeta */}
                                        <div className="confirm-row">
                                            <span className="confirm-label">Tarjeta</span>
                                            <span className="confirm-value">{getCardType(selectedCard.type)}</span>
                                        </div>
                                        
                                        {/* Numero de tarjeta */}
                                        <div className="confirm-row">
                                            <span className="confirm-label">Número de Tarjeta</span>
                                            <span className="confirm-value">{formatCardNumber(selectedCard.card_num)}</span>
                                        </div>

                                        {/* Monto */}
                                        <div className="confirm-row">
                                            <span className="confirm-label">Monto de Pago</span>
                                            <span className="confirm-amount-pay">
                                                {getCurrencySymbol(selectedAccount.currency_id) + ' ' + paymentAmount}
                                            </span>
                                        </div>
                                        
                                        {/* Cuenta de origen */}
                                        <div className="confirm-row">
                                            <span className="confirm-label">Desde Cuenta</span>
                                            <span className="confirm-value">{selectedAccount.description}</span>
                                        </div>
                                        
                                        {/* Fecha */}
                                        <div className="confirm-row">
                                            <span className="confirm-label">Fecha de Pago</span>
                                            <span className="confirm-value">{currentDate}</span>
                                        </div>

                                    </div>

                                    {/* Caja de informacion  */}
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

                            {/* Botón de envio de formulario */}
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