# API test cases

## Site API

1. Create a site with a valid payload returns `201 Created`.
2. Retrieve an existing site by id returns `200 OK`.
3. Retrieve a non-existing site returns `404 Not Found`.
4. Delete an existing site returns `204 No Content`.
5. Retrieve a deleted site returns `404 Not Found`.
6. Create a site with an invalid payload returns `400 Bad Request`.

## Building API

1. Create a building under an existing site returns `201 Created`.
2. Retrieve an existing building under the correct site returns `200 OK`.
3. Create a building for a non-existing site returns `404 Not Found`.
4. Delete an existing building returns `204 No Content`.
5. Retrieve a deleted building returns `404 Not Found`.
6. Create a building with an invalid payload returns `400 Bad Request`.

## Levels API

1. Import a single level for an existing building returns `201 Created`.
2. Import multiple levels for an existing building returns `201 Created`.
3. Import multiple levels with duplicate ordinals returns `400 Bad Request`.
4. Import levels for a non-existing building returns `404 Not Found`.
5. Import a level with an invalid payload returns `400 Bad Request`.
