import { createRoot } from "react-dom/client"
import "bootstrap/dist/css/bootstrap.css"
import "bootstrap"
import "bootstrap-icons/font/bootstrap-icons.css"
import "./css/BootstrapExtension.css"
import App from "./App.tsx"

createRoot(document.getElementById("root")!).render(
    <App />
);
