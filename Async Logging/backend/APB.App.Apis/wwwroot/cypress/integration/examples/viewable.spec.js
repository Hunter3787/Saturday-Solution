/// <reference types="cypress" />

context('Viewable Components', () => {
    beforeEach(() => {
      cy.visit('https://localhost:44317/')
    })

    it('contains page details', () => {
        cy.contains('Reviews and Ratings');
        cy.contains('Enter Username:');
        cy.contains('Star Rating:');
        cy.contains('Enter Review:');
        cy.contains('Word Count:');
        cy.contains('Enter Path to image file:');
    })
})