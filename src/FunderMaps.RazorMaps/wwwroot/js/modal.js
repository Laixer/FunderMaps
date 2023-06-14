export class Modal {
    constructor(options) {
        this.element = document.getElementById(options.element);
        this.element.getElementsByClassName("modal-close")[0].onclick = () => {
            this.element.style.display = "none";
            localStorage.setItem(`modal-closed-${this.element.dataset.id}`, true);
        };
    }

    show(id, html) {
        const hasAcceptedConsent = localStorage.getItem(`modal-closed-${id}`);
        if (!hasAcceptedConsent) {
            var consentText = this.element.getElementsByClassName("modal-content-body")[0];
            consentText.innerHTML = html;
            this.element.dataset.id = id;
            this.element.style.display = "block";
        }
    }
}