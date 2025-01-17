import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import Footer from "./components/layout/Footer.tsx";
import Navbar from "./components/layout/Navbar.tsx";
import Login_Register from "./pages/Account/Login_Register";
import NotFound from "./pages/Error/NotFound";
import Home from "./pages/Home.tsx";
import CreatePost from "./pages/Post/Create";
import PostId from "./pages/Post/Id";

const app = () => (
    <Router>
        <div className="container-xl d-flex flex-column">
            <Navbar />
            <main role="main" className="flex-grow-1 py-1 py-sm-2 py-md-3 container-xl">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/account/login" element={<Login_Register />} />
                    <Route path="/account/register" element={<Login_Register />} />
                    <Route path="/post/:id" element={<PostId />} />
                    <Route path="/post/create" element={<CreatePost />} />
                    <Route path="notfound" element={<NotFound />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </main>
            <Footer />
        </div>
    </Router>
);

export default app;