import { BrowserRouter as Router, Route, Routes } from "react-router-dom"
import Navbar from "./components/layout/Navbar.tsx"
import Footer from "./components/layout/Footer.tsx"
import Home from "./pages/Home.tsx"
import Login_Register from "./pages/Account/Login_Register"
import PostId from "./pages/Post/Id"
import CreatePost from "./pages/Post/Create"
import NotFound from "./pages/Error/NotFound"

const app = () => (
    <Router>
        <div className="container-xl">
            <Navbar />
            <main role="main" className="pb-3">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/account/login" element={<Login_Register />} />
                    <Route path="/account/register" element={<Login_Register />} />
                    <Route path="/post/:postId" element={<PostId />} />
                    <Route path="/post/create" element={<CreatePost />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </main>
            <Footer />
        </div>
    </Router>
);

export default app;