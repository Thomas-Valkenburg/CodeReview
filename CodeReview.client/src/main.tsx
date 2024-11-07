import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import { BrowserRouter as Router, Routes, Route } from "react-router-dom"
import "bootstrap/dist/css/bootstrap.css"
import Navbar from "./components/Navbar.tsx"
import Footer from "./components/Footer.tsx"
import Home from "./pages/Home.tsx"
import Login from "./pages/Account/login"

createRoot(document.getElementById("root")).render(
    <StrictMode>
        <Router>
            <div className="container">
                <Navbar/>
                <main role="main" className="pb-3">
                    <Routes>
                        <Route exact path="/" element={<Home />} />
                        <Route path="/account/login" element={<Login />} />
                    </Routes>
                </main>
                <Footer/>
            </div>
        </Router>
    </StrictMode>
);
