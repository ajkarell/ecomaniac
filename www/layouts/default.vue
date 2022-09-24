<template>
  <div class="h-screen flex flex-col">
    <nav class="w-full px-8 py-2 text-white bg-slate-700">
      <router-link to="/" class="text-xl font-bold">Ecomaniac</router-link>
    </nav>
    <div class="grow flex flex-col items-center justify-center">
      <slot />
    </div>
  </div>
</template>
