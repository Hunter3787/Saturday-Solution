/// <reference types="cypress" />

/// <summary>
/// References used from file: Solution Items/References.txt 
/// N/A: some examples were referenced from the examples provided by cypress' default files.
/// </summary>

// These tests will verify that the view page is correct.
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