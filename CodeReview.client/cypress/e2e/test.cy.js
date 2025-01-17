/* global cy */

describe("User stories", () => {
    const email = "email@email.com";
    const password = "Password123!";

    const title = "Cypress Test title";

    function login(e, p) {
        const url = cy.url();

        cy.visit("/account/login");

        cy.get("[data-id=email]").type(e ?? email);
        cy.get("[data-id=password]").type(p ?? password);

        cy.get("[data-id=submit]").click();
    }

    /*function logout() {
        const url = cy.url();

        try {
        cy.visit("/");

        cy.get(".dropdown > .d-none").click();
        cy.get(":nth-child(2) > .dropdown-item").click();

        cy.wait(1000);

        cy.visit(url.href ?? "/");
        } catch (e) {
        cy.visit(url.href ?? "/");
        }
    }*/

    beforeEach(() => {
        cy.visit("/");
    });

    it("Register_Fail_Not An Email", () => {
        cy.get("[href='/account/register']").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=email]").type("Not an email");
        cy.get("[data-id=password]").type(password);

        cy.get("[data-id=submit]").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=email-error]").should("have.text", "Input is not a valid email");
    });

    it("Register_Fail_Password Too Short", () => {
        cy.get("[href='/account/register']").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=email]").type(email);
        cy.get("[data-id=password]").type("Abc");

        cy.get("[data-id=submit]").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=password-error]").should("have.text", "Password must be at least 8 characters long");
    });

    it("Register_Fail_Password Too Long", () => {
        cy.get("[href='/account/register']").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=email]").type(email);
        cy.get("[data-id=password]").type("AVeryVeryVeryVeryLooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooongPassword");

        cy.get("[data-id=submit]").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=password-error]").should("have.text", "Password must be less then 16 characters long");
    });

    it("Register_Fail_Password No Uppercase", () => {
        cy.get("[href='/account/register']").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=email]").type(email);
        cy.get("[data-id=password]").type("lowercase");

        cy.get("[data-id=submit]").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=password-error]").should("have.text", "Password must contain at least one uppercase character");
    });

    it("Register_Fail_Password No Lowercase", () => {
        cy.get("[href='/account/register']").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=email]").type(email);
        cy.get("[data-id=password]").type("UPPERCASE");

        cy.get("[data-id=submit]").click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=password-error]").should("have.text", "Password must contain at least one lowercase character");
    });

    it("Register", () => {
        cy.get('[href="/account/register"]').click();

        cy.url().should("include", "/account/register");

        cy.get("[data-id=email]").type(email);
        cy.get("[data-id=password]").type(password);

        cy.get("[data-id=submit]").click();

        cy.wait(1000);

        cy.url().should("include", "/account/login");
    });

    it("Login_Fail_Invalid Email", () => {
        login("invalid@e.mail");

        cy.get("[data-id=login-error]").should("have.text", "Invalid email or password.");
    });

    it("Login_Fail_Invalid Password", () => {
        login(null, "InvalidPassword");

        cy.get("[data-id=login-error]").should("have.text", "Invalid email or password.");
    });

    it("Login", () => {
        login();

        cy.url().should("include", "/");

        cy.get("[data-id=navbar-account-dropdown]").click();

        cy.get("[data-id=navbar-email]").should("have.text", email);
        cy.get("[data-id=logout]").should("have.text", "Logout");
    });

    it("Create Post", () => {
        login();

        cy.get("#create").click();

        cy.get("[name=title]").type(title);

        cy.get("#content").type("Cypress Test content");

        cy.get("#submit").click();

        cy.wait(500);

        cy.url().should("include", "/");
    });

    it("View Post", () => {
        cy.visit("/post/26");

        cy.get("#title").should("have.text", title);

        cy.get(".public-DraftStyleDefault-block > [data-offset-key=9omk4-0-0] > span").should("have.text", "Cypress Test content");
    });

    it("View Post_Not Found", () => {
        cy.visit("/post/-1");

        cy.url().should("include", "/notfound");
    });

    it("Comment on Post", () => {
        login();

        cy.visit("/post/26");

        cy.wait(1000);

        cy.get("#comment-content").type("Cypress Test comment");

        cy.get("#submit").click();

        cy.get('.public-DraftStyleDefault-block > [data-offset-key="7jvja-0-0"] > span').should("have.text", "Cypress Test comment");
    });
});