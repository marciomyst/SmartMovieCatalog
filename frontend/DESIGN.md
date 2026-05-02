# AI Flix Design System

## Core Identity
- **Theme:** Dark Mode Only. Cinematic, premium, AI-focused.
- **Framework:** Tailwind CSS (Native Utility Classes). Do NOT generate custom color palettes or Material Design tokens in the config.
- **Iconography:** Material Symbols Outlined (Rounded/Light weight preferred).

## Design Tokens

### Colors (Strict Tailwind Palette)
- `bg-background`: `bg-zinc-950` (`#09090B`) - Main app background.
- `bg-surface`: `bg-zinc-900` (`#18181B`) - Default cards, standard panels.
- `bg-surface-elevated`: `bg-zinc-800` (`#27272A`) - Hover states, flipped cards, toasts.
- `border-default`: `border-zinc-800` - Standard borders.
- `border-focus`: `border-violet-500` - Active inputs.
- `text-primary`: `text-zinc-50` (`#FAFAFA`) - Headings, main body.
- `text-secondary`: `text-zinc-400` (`#A1A1AA`) - Subtitles, metadata.
- `text-muted`: `text-zinc-500` - Placeholders, disabled states.
- `accent-ai`: `text-violet-500` / `bg-violet-600` - AI actions, semantic search buttons.
- `accent-ai-gradient`: `bg-gradient-to-r from-indigo-500 to-purple-500` - Used strictly for the Logo and high-emphasis AI progress bars.
- `accent-success`: `text-emerald-400` / `bg-emerald-500/10` - AI Match percentage.
- `accent-warning`: `text-yellow-400` / `bg-yellow-400/10` - Warm visual tones/metadata.
- `accent-danger`: `bg-red-600` - Primary highlights like "Destaque" badges.

### Typography
- **Primary Font (Headings):** `Plus Jakarta Sans`, sans-serif. Used for high-impact display.
- **Secondary Font (Body/UI):** `Inter`, sans-serif. Used for readability.
- **H1 (Hero):** `font-['Plus_Jakarta_Sans'] text-5xl font-extrabold tracking-tighter text-zinc-50`
- **H2 (Section):** `font-['Plus_Jakarta_Sans'] text-2xl font-bold tracking-tight text-zinc-50`
- **Card Title:** `font-['Plus_Jakarta_Sans'] text-lg font-bold leading-tight`
- **Body:** `font-['Inter'] text-base font-normal text-zinc-300 leading-relaxed`
- **Small/Meta/Badge:** `font-['Inter'] text-xs font-semibold uppercase tracking-wider`

### Spacing & Layout
- **Page Container:** `max-w-7xl mx-auto px-6` (1280px max width).
- **Navbar Offset:** Main content must have `pt-24` to account for the fixed navbar.
- **Section Gap:** `py-16` or `space-y-16`.

### Borders & Effects
- **Radius - Cards:** `rounded-xl`
- **Radius - Buttons:** `rounded-lg`
- **Glassmorphism:** `bg-zinc-950/80 backdrop-blur-md border border-white/10` (Used for Navbar and Modals).

## Component Specifications

### 1. Top Navbar
- **Style:** Fixed to top, `z-50`, uses Glassmorphism.
- **Logo:** "AI Flix" using `font-black tracking-tighter text-transparent bg-clip-text accent-ai-gradient`.
- **Links:** `text-zinc-400 hover:text-zinc-50 transition-colors`. Active links get a `border-b-2 border-violet-500`.

### 2. RAG Search Bar (Header/Nav)
- **Container:** Rounded full, embedded in nav.
- **Style:** `bg-zinc-900 border border-zinc-800 flex items-center px-4 py-2 rounded-full`.
- **Input:** Transparent background, no focus ring (`focus:ring-0`), `text-sm`.

### 3. Movie Card (Standard)
- **Container:** `group relative flex flex-col rounded-xl overflow-hidden bg-zinc-900 border border-zinc-800 aspect-[2/3] cursor-pointer hover:scale-[1.02] transition-transform duration-300`.
- **Overlay:** `absolute inset-0 bg-gradient-to-t from-black/90 via-black/40 to-transparent`.
- **Content:** Absolute at bottom `p-4`.

### 4. Movie Card (AI Flip / Analysis)
- **Mechanics:** Requires custom CSS for 3D flip (`perspective: 1000px; transform-style: preserve-3d; backface-visibility: hidden;`).
- **Front:** Standard Movie Card, but includes a top-right AI badge `bg-violet-600/80 backdrop-blur`.
- **Back (Rotated 180deg):** `bg-zinc-800 border border-violet-500/30 p-4 flex flex-col justify-between`.
- **Back Content:** Displays "TOM VISUAL" tags and an "IA MATCH" progress bar using `accent-ai-gradient`.

### 5. SignalR Toast Notification (Async Processing)
- **Position:** `fixed bottom-8 right-8 z-50`.
- **Style:** `bg-zinc-800 border border-zinc-700 shadow-2xl rounded-lg p-4 flex items-start gap-4`.
- **Icon:** Inside a `bg-violet-500/10 text-violet-500 rounded-full p-2`.

### 6. Buttons
- **Primary:** `bg-gradient-to-r from-indigo-500 to-purple-600 text-white font-bold py-3 px-8 rounded-lg shadow-[0_0_20px_rgba(139,92,246,0.3)] hover:opacity-90 transition-all`.
- **Secondary:** `bg-zinc-800 hover:bg-zinc-700 text-zinc-50 border border-zinc-700 font-bold py-3 px-8 rounded-lg transition-all`.