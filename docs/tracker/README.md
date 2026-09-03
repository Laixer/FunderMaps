# Issue tracker export

Snapshot of this repository's GitHub issue tracker, taken 2026-09-03 before the
repository is archived (archiving makes issues read-only).

| file | contents |
|---|---|
| `labels.json` | all 11 labels (name, color, description) |
| `milestones.json` | all 28 milestones, open and closed |
| `open_issues.json` | the 16 issues still open on 2026-09-03 (trimmed: number, title, labels, milestone, body, author, dates) |
| `transfer-issues.sh` | recreates labels + referenced milestones in a target repo, then transfers every open issue with GitHub's native transfer (keeps comments, authors, timestamps, and leaves a redirect) |

The full dump (799 issues incl. bodies, 458 comments) is 6 MB and is kept out of
git; it lives on the ops VM under `~/fundermaps-csharp-tracker-export/` and can
be recreated with:

```sh
gh api "repos/Laixer/FunderMaps/issues?state=all&per_page=100" --paginate --slurp > issues.json
gh api "repos/Laixer/FunderMaps/issues/comments?per_page=100" --paginate --slurp > issue_comments.json
```

## Open issues on 2026-09-03

| # | opened | milestone | labels | title |
|---|---|---|---|---|
| [432](https://github.com/Laixer/FunderMaps/issues/432) | 2021-01-23 | Backlog | Feature, Database | Multiple foundationdamage cause  possible |
| [859](https://github.com/Laixer/FunderMaps/issues/859) | 2024-05-10 | 4.3 App-Maps | Enhancement, Database, Model | Ensure Orders are Created for Multiple Results in Backend |
| [912](https://github.com/Laixer/FunderMaps/issues/912) | 2024-09-24 |  | Action | AVG bepalen over RadarSat data |
| [930](https://github.com/Laixer/FunderMaps/issues/930) | 2025-02-20 |  | Enhancement | Serve single tileset databases |
| [984](https://github.com/Laixer/FunderMaps/issues/984) | 2026-05-20 |  | Enhancement, Feature, Database | Model V4 |
| [995](https://github.com/Laixer/FunderMaps/issues/995) | 2026-06-05 |  |  | Mapset owner |
| [1002](https://github.com/Laixer/FunderMaps/issues/1002) | 2026-07-16 |  |  | Expliciete reason-code toevoegen aan de analysis-productrespons wanneer geen risicoklasse kan worden geleverd |
| [1008](https://github.com/Laixer/FunderMaps/issues/1008) | 2026-07-23 |  |  | Multi org user voorkomt kunnen selecteren van reviewer bij inquiries |
| [1013](https://github.com/Laixer/FunderMaps/issues/1013) | 2026-08-26 |  |  | Ontbrekend BAG-PAND |
| [1014](https://github.com/Laixer/FunderMaps/issues/1014) | 2026-08-27 |  |  | Healthcheck beschikbaar maken voor NWWI en andere API-afnemers |
| [1015](https://github.com/Laixer/FunderMaps/issues/1015) | 2026-08-27 |  |  | Errorcode NO_BUILDING toevoegen voor BAG-statussen zonder bestaand pand |
| [1016](https://github.com/Laixer/FunderMaps/issues/1016) | 2026-08-27 |  |  | Zoekvoorziening maken voor PandIDs die aan testvoorwaarden voldoen |
| [1017](https://github.com/Laixer/FunderMaps/issues/1017) | 2026-08-27 |  |  | URL van brondocument toevoegen aan quickscan- en funderingsonderzoekresponse |
| [1018](https://github.com/Laixer/FunderMaps/issues/1018) | 2026-08-27 |  |  | NWWI gedurende zes maanden informeren over wijzigingen in het funderingsrisicorapport |
| [1019](https://github.com/Laixer/FunderMaps/issues/1019) | 2026-08-27 |  |  | Herstelkosten ook op verblijfsobjectniveau berekenen |
| [1020](https://github.com/Laixer/FunderMaps/issues/1020) | 2026-09-01 |  |  | Melden triage |
