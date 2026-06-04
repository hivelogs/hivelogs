# HiveLogs Design System

> Base file for Google Stitch UI generation.
> Use this document as the visual and UX foundation for every HiveLogs screen prompt.

---

## 1. Product Identity

HiveLogs is an observability platform for developers and small teams.

It centralizes:

- application logs
- metrics
- custom events
- environment health
- API/service telemetry

The interface should feel like a modern developer tool: clean, technical, fast, reliable, and easy to scan.

HiveLogs should not feel like an enterprise-heavy monitoring suite. It should feel lightweight, focused, and friendly for indie developers, small teams, and portfolio/startup projects.

---

## 2. Brand Personality

HiveLogs should communicate:

- reliability
- clarity
- control
- technical confidence
- calm monitoring
- developer friendliness
- lightweight observability

The UI should feel professional, but not cold.

Preferred mood:

- modern SaaS
- clean dashboard
- dark-first developer interface
- subtle gradients
- compact but readable data views
- high signal-to-noise ratio

Avoid:

- childish bee visuals
- excessive yellow/black bee theme
- overly playful illustrations
- cluttered enterprise dashboards
- too many neon effects
- generic admin template look

---

## 3. Logo Direction

The HiveLogs logo uses a geometric hexagonal symbol with layered inner shapes.

Visual associations:

- hive / hexagon / structured system
- logs grouped in organized layers
- infrastructure health
- monitored application core
- technology and reliability

The logo colors move from green at the top to cyan/blue and deeper blue at the bottom.

The UI should use the logo as inspiration, not as a literal decoration everywhere.

Use hexagonal motifs subtly in:

- empty states
- loading indicators
- app icon
- environment health cards
- background watermarks
- onboarding screens

Do not overuse honeycomb patterns.

---

## 4. Visual Theme

### Primary Theme

HiveLogs should be dark-first.

The default UI should use a dark background with layered surfaces.

Recommended background direction:

- app background: very dark navy / near-black blue
- sidebar: slightly lighter dark navy
- cards: dark blue-gray
- elevated panels: subtle border and shadow
- active states: cyan/blue glow or border
- success/healthy states: green accent
- warning states: amber
- error states: red/coral

The UI should also be adaptable to a light theme later, but all initial screens should be designed in dark mode.

---

## 5. Color Palette

Use the logo palette as the foundation.

### Core Colors

- Primary Cyan: used for main actions, active navigation, charts, selected filters
- Deep Blue: used for structure, sidebar, surfaces, gradients
- Hive Green: used for healthy status, success, uptime, positive signals
- Dark Navy: used for background and base layout
- Soft White: used for main text
- Muted Gray Blue: used for secondary text, borders, placeholders

### Semantic Colors

- Success: green
- Info: cyan/blue
- Warning: amber/yellow-orange
- Error: red/coral
- Critical: intense red or magenta-red
- Debug/Trace: muted purple or gray-blue

### Usage Rules

- Cyan should be the main interactive color.
- Green should mostly represent health and successful states.
- Blue should define the product atmosphere.
- Avoid making yellow the main brand color.
- Gradients should be subtle and inspired by the logo.

---

## 6. Typography

Use a modern, highly readable sans-serif font.

Recommended style:

- headings: strong, clean, slightly condensed if available
- body: readable and neutral
- numbers/metrics/log timestamps: use tabular numbers when possible
- code/log text: use a monospace font

Suggested font direction:

- Inter, Geist, Roboto, or similar for UI
- JetBrains Mono, IBM Plex Mono, or similar for logs/code snippets

Typography should support quick scanning.

---

## 7. Layout Principles

HiveLogs is a dashboard-heavy product, so layout should prioritize clarity.

Core layout:

- persistent left sidebar
- top header with environment/application context
- main content area with cards, tables, filters, and charts
- responsive grid for metrics and health cards

Desktop-first, but responsive.

Screen width assumptions:

- primary dashboard: desktop/tablet
- mobile support: simplified stacked layout
- logs table should become card/list style on small screens

Spacing:

- use generous spacing between sections
- compact spacing inside tables and log rows
- avoid dense walls of text
- keep filters close to the data they affect

---

## 8. Navigation Structure

Primary navigation should include:

- Overview
- Logs
- Metrics
- Events
- Applications
- Environments
- API Keys
- Settings

Optional later navigation:

- Alerts
- Deployments
- SDK Setup
- Usage
- Organization

The sidebar should show:

- HiveLogs logo
- current organization selector
- navigation items
- user/profile area at the bottom

The current application/environment selector should be visible near the top of the dashboard or header.

---

## 9. Dashboard Information Architecture

The Overview screen should answer:

- Is my application healthy?
- Are errors increasing?
- What happened recently?
- Which environment am I viewing?
- What are the most important logs/events right now?
- Are metrics trending normally?

Recommended overview sections:

1. Environment status summary
2. Key metric cards
3. Log volume chart
4. Error rate card/chart
5. Recent critical/error logs
6. Recent events
7. SDK/API ingestion status

---

## 10. Logs UX

Logs are the core feature and should feel powerful but simple.

Logs screen should include:

- search input
- level filter
- date/time range filter
- environment filter
- application/module filter
- source/service/module filter
- auto-refresh toggle
- table/list of logs
- log detail drawer

Each log row should show:

- timestamp
- level
- message
- application/module/source
- environment
- trace/request id if available
- compact metadata indicator
- exception indicator if available

Log levels should be visually distinct but not noisy.

Suggested level styling:

- Info: blue/cyan
- Warning: amber
- Error: red
- Critical: red with stronger emphasis
- Debug: muted purple/gray
- Trace: muted gray

Log detail drawer should show:

- full message
- exception stack trace
- metadata JSON
- request id / correlation id
- application
- environment
- module/source
- timestamp
- copy buttons

---

## 11. Metrics UX

Metrics should be simple in MVP 1.

Metrics screen should include:

- metric name selector
- date range selector
- environment selector
- tags filter
- line chart or area chart
- summary cards for latest, average, min, max

Charts should be readable and restrained.

Avoid overly complex analytics UI in the first version.

---

## 12. Events UX

Events are custom product/application events.

Events screen should include:

- event list
- event name filter
- date range filter
- environment filter
- properties preview
- event detail drawer

Each event row should show:

- timestamp
- event name
- application/environment
- key properties summary

---

## 13. Applications and Environments UX

Applications represent monitored projects.

Application list should show:

- name
- environments count
- last ingestion
- health/status indicator
- recent error count
- actions

Application detail should show:

- overview
- environments
- API keys
- SDK setup instructions
- recent telemetry

Environment detail should show:

- name
- API key status
- ingestion status
- last log received
- health status
- retention info later

---

## 14. API Key UX

API keys should feel secure and developer-friendly.

API key screen should include:

- environment name
- masked API key
- copy button
- regenerate button
- created date
- last used date
- status

Regenerate should be a dangerous action with confirmation.

---

## 15. Empty States

Empty states should be helpful and actionable.

Examples:

### No logs yet

Show:

- subtle hexagon/logo-inspired illustration
- title: “No logs received yet”
- explanation: “Install the HiveLogs SDK and send your first log from this environment.”
- CTA: “View SDK setup”
- secondary CTA: “Copy API key”

### No application

Show:

- title: “Create your first application”
- explanation: “Applications group logs, metrics, and events from one project.”
- CTA: “Create application”

Empty states should guide the developer to the next step.

---

## 16. Loading and Feedback

Loading states should use skeleton cards, not spinners everywhere.

Use:

- skeleton rows for logs table
- skeleton cards for dashboard metrics
- subtle progress indicator for auto-refresh
- toast notifications for success/error actions

---

## 17. Component Style

Use shadcn/ui-inspired components:

- cards
- buttons
- badges
- tabs
- tables
- sheets/drawers
- dialogs
- dropdowns
- command/search inputs
- toast notifications
- date range picker

Component shape:

- medium rounded corners
- subtle borders
- soft shadows
- clean focus states

Buttons:

- primary: cyan/blue
- secondary: dark surface with border
- destructive: red
- ghost: transparent hover

Badges:

- status and log levels should use compact badges

---

## 18. Data Density

HiveLogs should support developer workflows, so data density matters.

Default density:

- dashboard: medium
- logs table: compact but readable
- detail drawer: spacious
- charts: uncluttered

Avoid making the UI look like a marketing landing page once inside the app.

---

## 19. Accessibility

Design must support:

- good contrast in dark mode
- clear focus states
- readable font sizes
- non-color-only status communication
- icons plus labels where needed
- keyboard-friendly filters and drawers

---

## 20. Voice and Microcopy

Tone should be:

- clear
- direct
- developer-friendly
- calm
- helpful

Examples:

- “No logs received yet”
- “Last log received 2 minutes ago”
- “API key copied”
- “Regenerating this key will stop existing SDK clients until they are updated.”
- “Filter logs by message, level, module, or request id.”

Avoid:

- jokes in critical states
- overly cute bee references
- vague messages like “Something went wrong”

---

## 21. MVP Screen List

The first UI generation phase should cover:

1. Login
2. Register
3. Create organization
4. Application list
5. Create application
6. Application overview dashboard
7. Logs viewer
8. Log detail drawer
9. Metrics screen
10. Events screen
11. Environment/API key screen
12. SDK setup screen
13. Settings / organization settings

---

## 22. Screen Prompt Rules for Stitch

When generating screens in Stitch:

- Start each prompt by referencing this design.md.
- Generate one screen at a time.
- Keep the UI dark-first.
- Use the HiveLogs logo palette: green, cyan, blue, deep navy.
- Prefer realistic observability data.
- Prioritize developer workflow clarity.
- Use modern SaaS dashboard patterns.
- Avoid unnecessary illustrations except on empty states.
- Include realistic filters, timestamps, log messages, and statuses.
- Keep layouts consistent across screens.

---

## 23. Data Examples

Use realistic examples like:

Applications:

- Billing API
- Notification Worker
- Admin Web
- Checkout Service

Environments:

- Development
- Staging
- Production

Modules/Sources:

- Auth
- Orders
- Payments
- Notifications
- BackgroundJobs
- Database
- API Gateway

Log messages:

- “Payment authorization failed”
- “User login succeeded”
- “Order checkout completed”
- “Failed to send notification email”
- “Database query exceeded threshold”
- “Webhook received from payment provider”

Events:

- UserRegistered
- CheckoutStarted
- PaymentApproved
- PaymentFailed
- EmailNotificationQueued
- SubscriptionCreated

Metrics:

- request.duration
- requests.count
- errors.count
- queue.length
- worker.execution_time
- payment.approval_rate

---

## 24. Visual North Star

HiveLogs should look like a tool a developer would trust to monitor production systems, but simple enough to self-host and understand in minutes.

It should be:

- beautiful but practical
- technical but approachable
- powerful but not bloated
- dark, calm, and readable
- clearly connected to the hexagonal HiveLogs brand identity
