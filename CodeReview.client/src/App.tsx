import { BrowserRouter as Router, Route, Routes } from "react-router-dom"
import Navbar from "./components/Navbar.tsx"
import Footer from "./components/Footer.tsx"
import Home from "./pages/Home.tsx"
import Login from "./pages/Account/login"
import NotFound from "./pages/Error/NotFound"

const App = () => (
    <Router>
        <div className="container">
            <Navbar />
            <main role="main" className="pb-3">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/account/login" element={<Login />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </main>
            <Footer />
        </div>
    </Router>
);

export default App;