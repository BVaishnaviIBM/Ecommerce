
import { useEffect, useState } from "react";
import axios from "axios";

function Products() {
    const [products, setProducts] = useState([]);
    const [message, setMessage] = useState("");
    const [searchTerm, setSearchTerm] = useState("");

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const token = localStorage.getItem("token");

                const response = await axios.get(
                    "https://localhost:7248/api/Products",
                    {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                setProducts(
                    Array.isArray(response.data) ? response.data : []
                );
            } catch (error) {
                console.error("Error fetching products:", error);
                setMessage("Unable to load products.");
            }
        };

        fetchProducts();
    }, []);

    // Filter products by name or description
    const filteredProducts = products.filter((product) => {
        const search = searchTerm.trim().toLowerCase();

        return (
            product.productName?.toLowerCase().includes(search) ||
            product.description?.toLowerCase().includes(search)
        );
    });

    // Add a product to the cart
    const handleAddToCart = async (productId) => {
        setMessage("");

        try {
            const token = localStorage.getItem("token");

            await axios.post(
                "https://localhost:7248/api/Carts/items",
                {
                    cartId: 2,
                    productId: productId,
                    quantity: 1,
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            setMessage("Product added to cart successfully!");
        } catch (error) {
            console.error("Add to cart error:", error);

            setMessage(
                typeof error.response?.data === "string"
                    ? error.response.data
                    : "Unable to add product to cart."
            );
        }
    };

    return (
        <div className="products-container">
            <h2>Products</h2>

            {/* Search box */}
            <input
                className="search-input"
                type="text"
                placeholder="Search products by name or description..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
            />

            {/* Success or error message */}
            {message && (
                <p
                    className={
                        message.includes("successfully")
                            ? "success-message"
                            : "error-message"
                    }
                    role="status"
                >
                    {message}
                </p>
            )}

            {/* Product list */}
            {filteredProducts.length === 0 ? (
                <p>
                    {products.length === 0
                        ? "No products available."
                        : "No products match your search."}
                </p>
            ) : (
                <div className="products-grid">
                    {filteredProducts.map((product) => (
                        <div
                            className="product-card"
                            key={product.productId}
                        >
                            <h3>{product.productName}</h3>

                            <p>{product.description}</p>

                            <p>
                                <strong>Price:</strong>{" "}
                                ₹{Number(product.price).toFixed(2)}
                            </p>

                            <button
                                onClick={() =>
                                    handleAddToCart(product.productId)
                                }
                            >
                                Add to Cart
                            </button>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default Products;