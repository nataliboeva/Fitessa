# Logo Instructions

## How to Add Your Custom Logo

1. **Prepare Your Logo:**
   - Format: PNG (recommended) or SVG
   - Size: At least 200x200px for high quality
   - Background: Transparent or white background
   - Style: Simple, clean design that works on dark backgrounds

2. **File Names:**
   - Main logo: `fitessa-logo.png`
   - Alternative formats: `fitessa-logo.svg`, `fitessa-logo-white.png`

3. **Placement:**
   - Place your logo file in this directory: `Fitessa/wwwroot/images/logo/`
   - The system will automatically use it in the navbar and footer

4. **Logo Guidelines:**
   - Keep it simple and recognizable at small sizes
   - Ensure good contrast with dark backgrounds
   - Test how it looks in both navbar (40px height) and footer (30px height)

5. **Fallback:**
   - If no logo is found, the system will show just the text "Fitessa"
   - The logo will automatically be converted to white for the dark navbar

## Current Setup

The application is configured to:
- Display your logo in the navbar (left side)
- Show logo in the footer
- Automatically resize for different screen sizes
- Apply hover effects and animations
- Convert to white color for dark backgrounds

## Brand Customization

You can also customize:
- Colors in the theme customizer (palette icon)
- Font styles in the CSS variables
- Logo positioning and sizing in the CSS 