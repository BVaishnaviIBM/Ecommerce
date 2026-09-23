
import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";

function Navbar() {
    const [isLoggedIn, setIsLoggedIn] = useState(
        () => Boolean(localStorage.getItem("token"))
    );

    const navigate = useNavigate();

    useEffect(() => {
        const updateLoginState = () => {
            setIsLoggedIn(Boolean(localStorage.getItem("token")));
        };

        window.addEventListener("authChanged", updateLoginState);

        return () => {
            window.removeEventListener("authChanged", updateLoginState);
        };
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("token");
        setIsLoggedIn(false);
        navigate("/login");
    };

    return (
        <nav>
            <Link to="/login">Login</Link> |{" "}
            <Link to="/register">Register</Link>

            {isLoggedIn && (
                <>
                    {" | "}
                    <Link to="/products">Products</Link>
                    {" | "}
                    <Link to="/cart">Cart</Link>
                    {" | "}
                   
                    <Link to="/orders">Orders</Link>
                    {" | "}
                    <button onClick={handleLogout}>Logout</button>
                </>
            )}
        </nav>
    );
}

export default Navbar;