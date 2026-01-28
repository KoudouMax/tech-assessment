#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SQLPWD="${SQLPWD:-yourStrong(!)Password}"
DB_SERVER="${DB_SERVER:-host.docker.internal,5678}"
DB_NAME="${DB_NAME:-formation}"
DB_USER="${DB_USER:-sa}"
TOOLS_IMAGE="${TOOLS_IMAGE:-mcr.microsoft.com/mssql-tools}"

run_sqlcmd() {
  docker run --rm -v "$ROOT_DIR":/work -w /work "$TOOLS_IMAGE" \
    /opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER" -U "$DB_USER" -P "$SQLPWD" -C "$@"
}

run_sqlcmd -Q "IF DB_ID('$DB_NAME') IS NULL CREATE DATABASE $DB_NAME;"

run_sqlcmd -d "$DB_NAME" -i "WeChooz.TechAssessment.Database.SqlServer/Tables/Course.sql"
run_sqlcmd -d "$DB_NAME" -i "WeChooz.TechAssessment.Database.SqlServer/Tables/TrainingSession.sql"
run_sqlcmd -d "$DB_NAME" -i "WeChooz.TechAssessment.Database.SqlServer/Tables/Participant.sql"
run_sqlcmd -d "$DB_NAME" -i "WeChooz.TechAssessment.Database.SqlServer/Tables/SessionParticipant.sql"
run_sqlcmd -d "$DB_NAME" -i "WeChooz.TechAssessment.Database.SqlServer/Seed/Seed.sql"

echo "Schema applied to $DB_SERVER/$DB_NAME"
