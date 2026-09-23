
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";

function Register() {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        setMessage("");
        setLoading(true);

        try {
            await axios.post(
                "https://localhost:7248/api/Users/register",
                {
                    name: name,
                    email: email,
                    password: password,
                }
            );

            setMessage("Registration successful! Redirecting to login...");
            setTimeout(() => navigate("/login"), 1500);

        } catch (error) {
            console.error("Registration error:", error);
            console.error("Registration response:", error.response?.data);

            const data = error.response?.data;

            let errorMessage = "Registration failed. Please try again.";

            if (typeof data === "string") {
                errorMessage = data;
            } else if (data?.errors) {
                errorMessage = Object.values(data.errors)
                    .flat()
                    .join(" ");
            } else if (data?.message) {
                errorMessage = data.message;
            } else if (data?.title) {
                errorMessage = data.title;
            }

            setMessage(errorMessage);
        }
         finally {
            setLoading(false);
        }
    };

    return (
        <div className="register-container">
            <h2>Create Account</h2>

            <form onSubmit={handleRegister}>
                <div>
                    <label>Name</label>
                    <br />
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter your name"
                        required
                    />
                </div>

                <br />

                <div>
                    <label>Email</label>
                    <br />
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Enter your email"
                        required
                    />
                </div>

                <br />

                <div>
                    <label>Password</label>
                    <br />
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Create a password"
                        required
                    />
                </div>

                <br />

                <button type="submit" disabled={loading}>
                    {loading ? "Registering..." : "Register"}
                </button>
            </form>

            <p>{message}</p>

            <p>
                Already have an account? <Link to="/login">Login here</Link>
            </p>
        </div>
    );
}

export default Register;