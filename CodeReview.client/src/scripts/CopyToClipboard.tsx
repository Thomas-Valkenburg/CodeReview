import "react"

async function copyToClipboard(text: string) {
    // ReSharper disable once TsNotResolved
    await navigator.clipboard.writeText(text);

    alert("Copied to clipboard!");
}

export default copyToClipboard;