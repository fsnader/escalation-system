<template>
  <v-app id="inspire">
      <Sidebar v-model="drawer">
      </Sidebar>

    <v-app-bar app>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>XP Escalation System</v-toolbar-title>
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script>
import Sidebar from './components/Sidebar';

export default {
  name: 'App',
  data: () => ({drawer: null}),
  components: {Sidebar}
};
</script>
