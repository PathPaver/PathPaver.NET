# Create file
echo "token" > test-data/temp
curl -X POST --json @./test-data/login.json http://localhost:5231/api/v1/auth/login | \
    jq -r ".token" >> test-data/temp
echo "notAtoken!" >> test-data/temp

# Remove whitespace
sed -z 's/^\s*//; s/\s*$//' test-data/temp> test-data/token.csv

rm test-data/temp

# Run test
artillery run test-load.yaml