import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import { BrowserRouter as Router, Routes, Route } from "react-router-dom"
import "bootstrap/dist/css/bootstrap.css"
import Home from "./pages/Home.tsx"
import Navbar from "./components/Navbar.tsx"
import Footer from "./components/Footer.tsx"

createRoot(document.getElementById("root")).render(
    <StrictMode>
        <Router>
            <div className="container">
                <Navbar/>
                <main role="main" className="pb-3">
                    <Routes>
                        <Route exact path="/" element={<Home />}/>
                    </Routes>
                </main>
                <Footer/>
            </div>
        </Router>
    </StrictMode>
);
