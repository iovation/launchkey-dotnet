Feature: Factories can utilize single purpose keys
In order to communicate with the API
As a SDK
I can utilize single purpose keys

    @single_purpose_keys
    Scenario: Having both a configured encryption and signature key will work
        Given I am using single purpose keys
        When I perform an API call using single purpose keys
        Then there are no errors

    @single_purpose_keys
    Scenario: Using encryption key to sign will get the appropriate error
        Given I am using single purpose keys but I am using my encryption key to sign
        When I attempt an API call using single purpose keys
        Then the response status code should be 401

    @single_purpose_keys
    Scenario: Using signature key with no encryption key handles appropriate error
        Given I am using single purpose keys but I only set my signature key
        When I attempt an API call using single purpose keys
        Then no valid key will be available to decrypt response
