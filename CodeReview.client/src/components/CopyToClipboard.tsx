import react from "react"

async function copyToClipboard(text: string) {
    await navigator.clipboard.writeText(text);

    alert("Copied to clipboard!");
}

export default copyToClipboard;