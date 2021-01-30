<template>
  <div class="ma-5">
    <v-data-table
        :headers="headers"
        :items="desserts"
        sort-by="name"
        class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar
            flat
        >
          <v-toolbar-title><h4>Times</h4></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-dialog
              v-model="dialog"
              max-width="500px"
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                  color="primary"
                  dark
                  class="mb-2"
                  v-bind="attrs"
                  v-on="on"
              >
                Novo Time
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">{{ formTitle }}</span>
              </v-card-title>

              <v-card-text>
                <v-container>
                  <v-row>
                    <v-col
                        cols="12"
                    >
                      <v-text-field
                          v-model="editedItem.name"
                          label="nome"
                      ></v-text-field>
                    </v-col>
                    <v-col
                        cols="12"
                    >
                      <v-combobox
                          v-model="editedItem.membersName"
                          :items="members"
                          label="Membros"
                          multiple
                          dense
                      ></v-combobox>
                    </v-col>

                    <v-col
                        cols="12"
                    >
                      <v-combobox
                          v-model="editedItem.superior"
                          :items="superiores"
                          label="Quem escalonar"
                          dense
                      ></v-combobox>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="blue darken-1"
                    text
                    @click="close"
                >
                  Cancelar
                </v-btn>
                <v-btn
                    color="blue darken-1"
                    text
                    @click="save"
                >
                  Salvar
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
          <v-dialog v-model="dialogDelete" max-width="500px">
            <v-card>
              <v-card-title class="headline">Tem certeza eu deseja excluir esse Time?</v-card-title>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDelete">Cancelar</v-btn>
                <v-btn color="blue darken-1" text @click="deleteItemConfirm">OK</v-btn>
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:item.actions="{ item }">
        <v-icon
            small
            class="mr-2"
            @click="editItem(item)"
        >
          mdi-pencil
        </v-icon>
        <v-icon
            small
            @click="deleteItem(item)"
        >
          mdi-delete
        </v-icon>
      </template>
      <template v-slot:no-data>
        <v-btn
            color="primary"
            @click="initialize"
        >
          Reset
        </v-btn>
      </template>
    </v-data-table>
  </div>
</template>

<script>
export default {
  name: 'Team',
  data: () => ({
    dialog: false,
    dialogDelete: false,
    headers: [
      {text: 'Nome', value: 'name'},
      {text: 'Membros', value: 'membersName'},
      {text: 'Quem escalonar?', value: 'superior'},
      {text: 'Ações', value: 'actions', sortable: false},
    ],
    desserts: [],
    editedIndex: -1,
    editedItem: {
      name: '',
      members:[ 'Alberto dev1', 'Bruno dev2', 'Caio dev3', 'Denis dev4', 'Camila techlead', 'Danilo gerente', 'Eduarda coordenadora','Benchimol'],
      membersName:[],
      superior: ''
    },
    defaultItem: {
      name: '',
      members:[ 'Alberto dev1', 'Bruno dev2','Caio dev3', 'Denis dev4', 'Camila techlead', 'Danilo gerente', 'Eduarda coordenadora','Benchimol'],
      membersName:[],
      superior: ''
    },
  }),

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? 'Inserir Time' : 'Editar Time'
    },
  },

  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDelete()
    },
    getTeams() {
        console.log('oi')
        if(this.desserts.members)
          return this.desserts.members.map(x => x.name);
        else
          return [];
    }
  },

  created() {
    this.initialize()
  },

  methods: {
    initialize() {
      this.superiores = ['Devs PIX', 'techlead PIX', 'gerente Squad Pagamento Instantâneos']
      this.members =[ 'Alberto dev1', 'Bruno dev2','Camila techlead', 'Danilo gerente', 'Eduarda coordenadora', 'Benchimol'     ],
      this.desserts = [
        {
          name: 'Devs PIX',
          membersName:['Alberto dev1', 'Bruno dev2'],
          superior: 'techlead PIX'
        },
        {
          name: 'techlead PIX',
          membersName:['Camila techlead'],
          superior: 'gerente Squad Pagamento Instantâneos'
        },
        {
          name: 'gerente Squad Pagamento Instantâneos',
          membersName:['Danilo gerente'],
          superior: '(Atendente do chamado)'
        },
      ]
    },

    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialog = true
    },

    deleteItem(item) {
      this.editedIndex = this.desserts.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialogDelete = true
    },

    deleteItemConfirm() {
      this.desserts.splice(this.editedIndex, 1)
      this.closeDelete()
    },

    close() {
      this.dialog = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },

    closeDelete() {
      this.dialogDelete = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    },

    save() {
      if (this.editedIndex > -1) {
        Object.assign(this.desserts[this.editedIndex], this.editedItem)
      } else {
        this.desserts.push(this.editedItem)
      }
      this.close()
    },
  },
}
</script>