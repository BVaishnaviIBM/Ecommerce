
import { useEffect, useState } from "react";
import axios from "axios";

const API_URL = "https://localhost:7248/api";

function Cart() {
    const [cart, setCart] = useState(null);
    const [message, setMessage] = useState("");
    const [loading, setLoading] = useState(true);
    const [checkingOut, setCheckingOut] = useState(false);

    const [paymentMethod, setPaymentMethod] = useState("UPI");
    const [payment, setPayment] = useState(null);

    const getHeaders = () => ({
        Authorization: `Bearer ${localStorage.getItem("token")}`,
    });

    const fetchCart = async () => {
        try {
            const response = await axios.get(`${API_URL}/Carts`, {
                headers: getHeaders(),
            });

            setCart(response.data);
            setMessage("");
        } catch (error) {
            console.error("Error fetching cart:", error);
            setMessage("Unable to load cart.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchCart();
    }, []);

    const refreshCart = async () => {
        try {
            const response = await axios.get(`${API_URL}/Carts`, {
                headers: getHeaders(),
            });

            setCart(response.data);
        } catch (error) {
            console.error("Error refreshing cart:", error);
            setMessage("Unable to refresh cart.");
        }
    };

    const getErrorMessage = (error, fallback) => {
        return typeof error.response?.data === "string"
            ? error.response.data
            : error.response?.data?.title ||
            `${fallback} Status: ${error.response?.status || "Network error"
            }`;
    };

    const updateQuantity = async (itemId, quantity) => {
        if (quantity < 1) {
            setMessage("Quantity must be at least 1.");
            return;
        }

        try {
            await axios.put(
                `${API_URL}/Carts/items/${itemId}`,
                { quantity },
                { headers: getHeaders() }
            );

            setMessage("Quantity updated successfully!");
            await refreshCart();
        } catch (error) {
            console.error("Update quantity error:", error);
            setMessage(getErrorMessage(error, "Unable to update quantity."));
        }
    };

    const removeItem = async (itemId) => {
        try {
            await axios.delete(`${API_URL}/Carts/items/${itemId}`, {
                headers: getHeaders(),
            });

            setMessage("Item removed successfully!");
            await refreshCart();
        } catch (error) {
            console.error("Remove item error:", error);
            setMessage(getErrorMessage(error, "Unable to remove item."));
        }
    };

    const handleCheckout = async () => {
        try {
            setCheckingOut(true);
            setMessage("");
            setPayment(null);

            const response = await axios.post(
                `${API_URL}/Orders/checkout`,
                null,
                {
                    params: { paymentMethod },
                    headers: getHeaders(),
                }
            );

            const order = response.data;
            console.log("Checkout response:", order);

            setMessage(
                `Checkout successful! Order #${order.orderId} has been placed.`
            );

            
            try {
                const paymentResponse = await axios.get(
                    `${API_URL}/Payments/order/${order.orderId}`,
                    { headers: getHeaders() }
                );

                console.log("Payment API response:", paymentResponse.data);

                setPayment(paymentResponse.data);
            } catch (paymentError) {
                console.error("Unable to retrieve payment:", paymentError);
                setMessage(
                    `Order #${order.orderId} was placed, but payment details could not be retrieved.`
                );
            }

            await refreshCart();
        } catch (error) {
            console.error("Checkout error:", error);
            setMessage(getErrorMessage(error, "Checkout failed."));
        } finally {
            setCheckingOut(false);
        }
    };

    if (loading) {
        return <p>Loading your cart...</p>;
    }

    const cartItems = cart?.cartItems ?? [];

    const totalAmount = cartItems.reduce(
        (total, item) =>
            total + Number(item.product?.price ?? 0) * item.quantity,
        0
    );

    return (
        <div className="cart-container">
            <h2>My Cart</h2>

            {message && (
                <p className="success-message" role="status">
                    {message}
                </p>
            )}

            {cartItems.length === 0 && <p>Your cart is empty.</p>}

            {cartItems.map((item) => (
                <div className="cart-item" key={item.cartItemId}>
                    <h3>{item.product.productName}</h3>

                    <p>Description: {item.product.description}</p>
                    <p>Price: ₹{Number(item.product.price).toFixed(2)}</p>
                    <p>Quantity: {item.quantity}</p>
                    <p>
                        Subtotal: ₹
                        {(Number(item.product.price) * item.quantity).toFixed(2)}
                    </p>

                    <button
                        onClick={() =>
                            updateQuantity(item.cartItemId, item.quantity - 1)
                        }
                        disabled={item.quantity <= 1}
                    >
                        -
                    </button>

                    <button
                        onClick={() =>
                            updateQuantity(item.cartItemId, item.quantity + 1)
                        }
                    >
                        +
                    </button>

                    <button onClick={() => removeItem(item.cartItemId)}>
                        Remove
                    </button>

                    <hr />
                </div>
            ))}

            {cartItems.length > 0 && (
                <div className="checkout-section">
                    <h3>Total: ₹{totalAmount.toFixed(2)}</h3>

                    <label htmlFor="paymentMethod">Payment method:</label>

                    <select
                        id="paymentMethod"
                        value={paymentMethod}
                        onChange={(event) =>
                            setPaymentMethod(event.target.value)
                        }
                        disabled={checkingOut}
                    >
                        <option value="UPI">UPI</option>
                        <option value="Cash on Delivery">
                            Cash on Delivery
                        </option>
                        <option value="Card">Card</option>
                    </select>

                    <p>
                        <small>
                            This is a simulated payment flow. No real payment
                            will be charged.
                        </small>
                    </p>

                    <button
                        onClick={handleCheckout}
                        disabled={checkingOut}
                    >
                        {checkingOut ? "Processing..." : "Place Order"}
                    </button>
                </div>
            )}

            {payment && (
                <div className="payment-details">
                    <h3>Payment Details</h3>
                    <p>Payment ID: {payment.paymentId}</p>
                    <p>Order ID: {payment.orderId}</p>
                    <p>
                        Amount: ₹{Number(payment.amount).toFixed(2)}
                    </p>
                    <p>Payment Method: {payment.paymentMethod}</p>
                    <p>Payment Status: {payment.paymentStatus}</p>
                    <p>
                        Payment Date:{" "}
                        {payment.paymentDate
                            ? new Date(payment.paymentDate).toLocaleString()
                            : "Not available"}
                    </p>
                </div>
            )}
        </div>
    );
}

export default Cart;