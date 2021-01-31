<template>
  <v-navigation-drawer app>
    <v-list-item>
      <v-list-item-content>
        <v-list-item-title class="title">
          Adalberto Oliveira
        </v-list-item-title>
        <v-list-item-subtitle>
          Analista de incidente Sr.
        </v-list-item-subtitle>
      </v-list-item-content>
    </v-list-item>

    <v-divider></v-divider>

    <v-list
        v-for="category in categories"
        :key="category.title"
        dense
        nav
    >
      <v-subheader>{{ category.title}}</v-subheader>

      <v-list-item
          v-for="item in category.items"
          :key="item.title"
          :to="item.link"
          link
      >
        <v-list-item-icon>
          <v-icon>{{ item.icon }}</v-icon>
        </v-list-item-icon>

        <v-list-item-content>
          <v-list-item-title>{{ item.title }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
export default {
  name: "Sidebar",
  data() {
    return {
      categories: [
        {
          title: "Cadastros",
          items: [
            {title: 'Funcionários', icon: 'mdi-account', link: '/funcionarios'},
            {title: 'Times', icon: 'mdi-account-group', link: '/times'},
            {title: 'Escala de acionamento', icon: 'mdi-calendar-account', link: '/escala-de-acionamento'},
          ]
        },
        {
          title: "Acompanhamento",
          items: [
            {title: 'Incidentes', icon: 'mdi-view-dashboard', link: '/incidentes'},
          ]
        },
      ],
      right: null,
    }
  },
}
</script>