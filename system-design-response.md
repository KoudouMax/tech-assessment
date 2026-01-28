# Reponse - Conception & architecture

## 1) Points a corriger / ameliorer pour etre "prod-ready"

### Securite & conformite
- **Authentification reelle** (au lieu d'un login sans mot de passe).
  - Remediation : OIDC/SSO (Azure AD/Keycloak), MFA si necessaire, politique de mot de passe si comptes locaux.
- **CSRF** pour les endpoints mutateurs (cookies d'auth).
  - Remediation : antiforgery token cote API + header cote front, rotation par session.
- **Protection XSS** pour le rendu Markdown.
  - Remediation : HTML desactive + sanitizer (ex: Ganss.Xss) + CSP restrictive.
- **Cookies securises**.
  - Remediation : `HttpOnly`, `Secure`, `SameSite=Lax/Strict`, expiration + rotation de cle.
- **RBAC fin**.
  - Remediation : policies par action, tests de permissions, audit des acces.
- **Headers de securite**.
  - Remediation : HSTS, CSP, X-Content-Type-Options, Referrer-Policy, etc.
- **Rate limiting**.
  - Remediation : limites par IP/token sur endpoints publics.

### Donnees & integrite
- **Schema versionne** (pas de "drop full").
  - Remediation : pipeline DACPAC SSDT (ou Flyway/DbUp) en CI/CD.
- **Concurrence capacite**.
  - Remediation : transaction + verrous, contrainte unique, check en base.
- **Validation metier stricte**.
  - Remediation : FluentValidation + regles metier (dates futures, capacite, doublons).
- **Sauvegardes/restore**.
  - Remediation : backup automatise + test de restauration.

### Architecture & qualite
- **Contrats API clairs**.
  - Remediation : OpenAPI (Swagger), versioning (ex: /v1), erreurs standardisees.
- **Tests insuffisants**.
  - Remediation : tests unitaires metier, integration API, tests UI critiques.
- **Observabilite**.
  - Remediation : logs structures, correlation ID, metrics, traces OTel.
- **Qualite code**.
  - Remediation : linting, analyzers, code review, CI gate.

### Performance & scalabilite
- **Pagination** partout (public + admin).
  - Remediation : `page/pageSize` + index sur champs filtres.
- **Cache** des listes publiques.
  - Remediation : cache Redis, invalidation sur mutation, TTL court.
- **Charges & N+1**.
  - Remediation : projections et requetes optimisees, pas d'Include inutile.
- **Debits & SLA**.
  - Remediation : load tests, budgets latence, alerting.

### Deploiement & operations
- **CI/CD**.
  - Remediation : build, tests, publish, deploy, rollback.
- **Secrets**.
  - Remediation : vault (Azure Key Vault / Hashicorp), jamais en clair.
- **Infra as Code**.
  - Remediation : Terraform/Bicep.

---

## 2) Fonctionnalite d'inscription client apres achat

### Objectif
Permettre a un client final de **s'inscrire a une session** apres achat, **sans compte**, tout en limitant les fraudes.

### Flux utilisateur
1) **Vente** : un commercial cree une invitation liee a une vente (saleId, sessionId).
2) **Invitation** : email avec lien unique.
3) **Inscription** : client ouvre le lien, saisit ses infos.
4) **Verification** : OTP par email ou SMS.
5) **Confirmation** : email + calendrier.

### Securisation sans login
- **Token unique** (opaque ou JWT signe)
  - Expiration courte (ex: 7 jours).
  - Usage unique (one-time).
  - Stockage du hash du token en base.
- **OTP** pour valider l'email.
- **Rate limiting** par IP + par token.
- **Audit** : journaliser tentatives et usages.
- **Protection anti-replay** : token marque "used" des qu'il est confirme.

### Modele de donnees minimal
- `RegistrationInvite` : id, saleId, sessionId, email, tokenHash, expiresAt, usedAt
- `RegistrationAttempt` : inviteId, ip, userAgent, createdAt, status
- `Registration` : sessionId, participantId, createdAt

### API proposee
- `POST /_api/public/registration/invite` (commercial)
- `GET /registration/{token}` (page publique)
- `POST /_api/public/registration/confirm` (OTP + payload)

### Regles metier
- Session ouverte, pas passee.
- Capacite strictement respectee.
- Email unique par session.
- Invitations expirees ou deja utilisees -> 410/400.

---

## 3) Resume

Le projet est **fonctionnel** pour le test, mais pour etre "prod-ready" il faut :
- securiser (auth reelle, CSRF, XSS, headers),
- industrialiser la DB (pipeline DACPAC + backups),
- renforcer qualite (tests + observabilite + contrats API),
- gerer performance (pagination, cache, indexes).

La fonctionnalite d'inscription sans login peut etre securisee avec **tokens uniques + OTP** + rate limiting + audit.

---

## 4) Mini schema ASCII des flux

Flux d'inscription apres achat :

```
[Commercial] -> POST /_api/public/registration/invite
        |                          |
        |                     [RegistrationInvite]
        v                          |
   Email invite  <-----------------+
        |
        v
[Client] -> GET /registration/{token} -> page publique
        |
        v
[Client] -> POST /_api/public/registration/confirm (OTP + payload)
        |
        v
[Validation] -> token unique + capacite + OTP
        |
        v
[Registration] + confirmation email
```
