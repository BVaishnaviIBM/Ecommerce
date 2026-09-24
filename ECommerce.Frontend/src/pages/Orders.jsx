import { HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect, useState } from "react";
import axios from "axios";

function Orders() {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState("");
    const [selectedOrder, setSelectedOrder] = useState(null);
    const [detailsLoading, setDetailsLoading] = useState(false);
    const [notification, setNotification] = useState("");

    {
        notification && (
            <p className="success-message" role="status">
                {notification}
            </p>
        )
    }

    useEffect(() => {
        let connection;

        const connectToOrderHub = async () => {
            try {
                connection = new HubConnectionBuilder()
                    .withUrl("https://localhost:7248/orderHub")
                    .withAutomaticReconnect()
                    .build();

                connection.on("OrderUpdated", (orderId, status) => {
                    setNotification(
                        `Order #${orderId} has been updated. Status: ${status}`
                    );
                });

                await connection.start();
                console.log("Connected to OrderHub");
            } catch (error) {
                console.error("SignalR connection failed:", error);
            }
        };

        connectToOrderHub();

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, []);

    useEffect(() => {
        let isMounted = true;

        const loadOrders = async () => {
            try {
                const token = localStorage.getItem("token");

                const response = await axios.get(
                    "https://localhost:7248/api/Orders",
                    {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                if (isMounted) {
                    setOrders(
                        Array.isArray(response.data) ? response.data : []
                    );
                }
            } catch (error) {
                console.error("Fetch orders error:", error);

                if (isMounted) {
                    setMessage(
                        typeof error.response?.data === "string"
                            ? error.response.data
                            : error.response?.data?.message ||
                            "Unable to load orders. Please try again."
                    );
                }
            } finally {
                if (isMounted) {
                    setLoading(false);
                }
            }
        };

        loadOrders();

        return () => {
            isMounted = false;
        };
    }, []);

    const viewOrderDetails = async (orderId) => {
        setDetailsLoading(true);
        setMessage("");

        try {
            const token = localStorage.getItem("token");

            const response = await axios.get(
                `https://localhost:7248/api/Orders/${orderId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            setSelectedOrder(response.data);
        } catch (error) {
            console.error("Order details error:", error);

            setMessage(
                typeof error.response?.data === "string"
                    ? error.response.data
                    : "Unable to load order details."
            );
        } finally {
            setDetailsLoading(false);
        }
    };

    const formatDate = (dateValue) => {
        if (!dateValue) return "Not available";

        const date = new Date(dateValue);

        if (Number.isNaN(date.getTime())) return dateValue;

        return date.toLocaleString();
    };

    const formatPrice = (amount) => {
        if (amount == null) return "Not available";

        return Number(amount).toLocaleString("en-IN", {
            style: "currency",
            currency: "INR",
        });
    };

    if (loading) {
        return <p>Loading your orders...</p>;
    }

    return (
        <div className="orders-container">
            <h2>My Orders</h2>

            {message && <p role="alert">{message}</p>}

            {orders.length === 0 ? (
                <p>You haven't placed any orders yet.</p>
            ) : (
                <div className="orders-list">
                    {orders.map((order) => (
                        <div className="order-card" key={order.orderId}>
                            <h3>Order #{order.orderId}</h3>

                            <p>
                                <strong>Status:</strong>{" "}
                                {order.status ?? "Not available"}
                            </p>

                            <p>
                                <strong>Total:</strong>{" "}
                                {formatPrice(order.totalAmount)}
                            </p>

                            <p>
                                <strong>Order date:</strong>{" "}
                                {formatDate(order.orderDate)}
                            </p>

                            <button
                                onClick={() => viewOrderDetails(order.orderId)}
                                disabled={detailsLoading}
                            >
                                View Details
                            </button>
                        </div>
                    ))}
                </div>
            )}

            {detailsLoading && <p>Loading order details...</p>}

            {selectedOrder && (
                <div className="order-details">
                    <h2>Order Details #{selectedOrder.orderId}</h2>

                    <p>
                        <strong>Status:</strong>{" "}
                        {selectedOrder.status ?? "Not available"}
                    </p>

                    <p>
                        <strong>Total:</strong>{" "}
                        {formatPrice(selectedOrder.totalAmount)}
                    </p>

                    <p>
                        <strong>Order date:</strong>{" "}
                        {formatDate(selectedOrder.orderDate)}
                    </p>

                    {Array.isArray(selectedOrder.orderItems) && (
                        <>
                            <h3>Items</h3>

                            {selectedOrder.orderItems.length === 0 ? (
                                <p>No item details available.</p>
                            ) : (
                                selectedOrder.orderItems.map((item, index) => (
                                    <div key={item.orderItemId ?? index}>
                                        <p>
                                            {item.product?.productName ??
                                                `Product #${item.productId}`}
                                        </p>

                                        <p>Quantity: {item.quantity}</p>

                                        <p>
                                            Price:{" "}
                                            {formatPrice(
                                                item.price ?? item.unitPrice
                                            )}
                                        </p>
                                    </div>
                                ))
                            )}
                        </>
                    )}

                    <button onClick={() => setSelectedOrder(null)}>
                        Close Details
                    </button>
                </div>
            )}
        </div>
    );
}


export default Orders;