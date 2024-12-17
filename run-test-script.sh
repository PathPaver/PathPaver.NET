curl -X POST --json @./test-data/login.json http://localhost:5231/api/v1/auth/login > test-data/result.json
# artillery run test-load.yaml