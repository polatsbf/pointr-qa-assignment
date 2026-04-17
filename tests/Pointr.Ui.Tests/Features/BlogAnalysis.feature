Feature: Blog article word analysis
  In order to validate the blog page
  As a QA engineer
  The latest articles should be available and analyzable in supported browsers

  Scenario Outline: Verify all blog articles have loaded
    Given I open the Pointr blog in "<browser>"
    When I collect all article links
    Then all articles should be loaded successfully

    Examples:
      | browser  |
      | chromium |
      | firefox  |

  Scenario Outline: Save the top 5 repeated words from the latest 3 articles
    Given I open the Pointr blog in "<browser>"
    When I collect the latest 3 article contents
    And I calculate the most repeated 5 words
    Then the latest 3 articles should be loaded successfully
    And the top 5 words should be saved into a text file for "<browser>"

    Examples:
      | browser  |
      | chromium |
      | firefox  |
