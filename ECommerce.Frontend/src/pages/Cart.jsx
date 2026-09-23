
import { useEffect, useState } from "react";
import axios from "axios";

function Cart() {
    const [cart, setCart] = useState(null);
    const [message, setMessage] = useState("");
    const [loading, setLoading] = useState(true);
    const [checkingOut, setCheckingOut] = useState(false);

    // Fetch the logged-in user's cart
    const fetchCart = async () => {
        try {
            const token = localStorage.getItem("token");

            const response = await axios.get(
                "https://localhost:7248/api/Carts",
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            setCart(response.data);
            setMessage("");
        } catch (error) {
            console.error("Error fetching cart:", error);
            setMessage("Unable to load cart.");
        } finally {
            setLoading(false);
        }
    };

    // Load cart when the component mounts
    useEffect(() => {
        fetchCart();
    }, []);

    // Refresh cart after updating or removing an item
    const refreshCart = async () => {
        try {
            const token = localStorage.getItem("token");

            const response = await axios.get(
                "https://localhost:7248/api/Carts",
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            setCart(response.data);
        } catch (error) {
            console.error("Error refreshing cart:", error);
            setMessage("Unable to refresh cart.");
        }
    };

    // Increase or decrease item quantity
    const updateQuantity = async (itemId, quantity) => {
        if (quantity < 1) {
            setMessage("Quantity must be at least 1.");
            return;
        }

        try {
            const token = localStorage.getItem("token");

            await axios.put(
                `https://localhost:7248/api/Carts/items/${itemId}`,
                {
                    quantity: quantity,
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            setMessage("Quantity updated successfully!");
            await refreshCart();
        } catch (error) {
            console.error("Update quantity error:", error);
            console.log("Status:", error.response?.status);
            console.log("Response data:", error.response?.data);

            setMessage(
                typeof error.response?.data === "string"
                    ? error.response.data
                    : error.response?.data?.title ||
                    `Unable to update quantity. Status: ${error.response?.status || "Network error"
                    }`
            );
        }
    };

    // Remove an item from the cart
    const removeItem = async (itemId) => {
        try {
            const token = localStorage.getItem("token");

            await axios.delete(
                `https://localhost:7248/api/Carts/items/${itemId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            setMessage("Item removed successfully!");
            await refreshCart();
        } catch (error) {
            console.error("Remove item error:", error);
            console.log("Status:", error.response?.status);
            console.log("Response data:", error.response?.data);

            setMessage(
                typeof error.response?.data === "string"
                    ? error.response.data
                    : error.response?.data?.title ||
                    `Unable to remove item. Status: ${error.response?.status || "Network error"
                    }`
            );
        }
    };

    // Place an order using the checkout API
    const handleCheckout = async () => {
        try {
            setCheckingOut(true);
            setMessage("");

            const token = localStorage.getItem("token");

            const response = await axios.post(
                "https://localhost:7248/api/Orders/checkout",
                null,
                {
                    params: {
                        paymentMethod: "UPI",
                    },
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            console.log("Checkout response:", response.data);

            setMessage("Checkout successful! Your order has been placed.");

            await refreshCart();
        } catch (error) {
            console.error("Checkout error:", error);
            console.log("Status:", error.response?.status);
            console.log("Response data:", error.response?.data);

            setMessage(
                typeof error.response?.data === "string"
                    ? error.response.data
                    : error.response?.data?.title ||
                    `Checkout failed. Status: ${error.response?.status || "Network error"
                    }`
            );
        } finally {
            setCheckingOut(false);
        }
    };

    // Loading state
    if (loading) {
        return <p>Loading your cart...</p>;
    }

    return (
        <div>
            <h2>My Cart</h2>

            {message && <p>{message}</p>}

            {cart && cart.cartItems.length === 0 && (
                <p>Your cart is empty.</p>
            )}

            {cart &&
                cart.cartItems.map((item) => (
                    <div key={item.cartItemId}>
                        <h3>{item.product.productName}</h3>

                        <p>Description: {item.product.description}</p>
                        <p>Price: ₹{item.product.price}</p>
                        <p>Quantity: {item.quantity}</p>

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

                        <button
                            onClick={() => removeItem(item.cartItemId)}
                        >
                            Remove
                        </button>

                        <hr />
                    </div>
                ))}

            {cart && cart.cartItems.length > 0 && (
                <button
                    onClick={handleCheckout}
                    disabled={checkingOut}
                >
                    {checkingOut ? "Processing..." : "Checkout"}
                </button>
            )}
        </div>
    );
}

export default Cart;