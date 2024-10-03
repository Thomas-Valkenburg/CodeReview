import Navigation from "./Navigation"
import Footer from "./Footer"

const Layout = ({ children }) => {
    return (
        <div className="container">
            <Navigation/>
            <main role="main" className="pb-3">
                {children}
            </main>
            <Footer />
        </div>
    );
}

export default Layout