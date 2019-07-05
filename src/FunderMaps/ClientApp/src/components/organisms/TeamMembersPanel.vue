<template>
  <div>
   <div v-if="(isSuperUser() || isAdmin()) && orgUsersExcludingSelf">
    <div class="panel px-4 py-3" style="width: 300px">
      <h2 class="font-weight-bold mt-1 mb-4">Teamleden</h2>
      <TeamMember 
        v-for="(member, index) in orgUsersExcludingSelf" 
        :member="member"
        :key="index"
        @edit="handleEdit"
        @remove="handleRemove" />
    </div>
   
    <TeamMemberModal 
      :id="editUserId" 
      :orgId="orgId" />

    <RemoveTeamMemberModal 
      :id="editUserId" 
      :orgId="orgId" />
  
    </div>
    <b-button 
      variant="primary" 
      class="SubmitButton font-weight-bold mt-4" 
      size="lg"
      @click="handleCreate"
      pill>
      <span class="d-inline-block my-2">
        Gebruiker registreren
      </span>
    </b-button>
    
    <NewTeamMemberModal 
      :orgId="orgId" />
  
  </div>
</template>

<script>
import { isSuperUser, isAdmin } from 'service/auth'
import { mapGetters, mapActions } from 'vuex'

import TeamMember from 'molecule/TeamMember'
import TeamMemberModal from 'organism/TeamMemberModal'
import NewTeamMemberModal from 'organism/NewTeamMemberModal'
import RemoveTeamMemberModal from 'organism/RemoveTeamMemberModal'

import { getUserId } from 'service/auth'

export default {
  components: {
    TeamMember, TeamMemberModal, NewTeamMemberModal, RemoveTeamMemberModal
  },
  data() {
    return {
      editUserId: ''
    }
  },
  computed: {
    ...mapGetters('org', [
      'getOrgId'
    ]),
    ...mapGetters('orgUsers', [
      'orgUsers'
    ]),
    orgUsersExcludingSelf() {
      let id = getUserId()
      return this.orgUsers ? this.orgUsers.filter(
        user => user.user.id !== id
      ) : false
    },
    orgId() {
      if (isAdmin() && this.$route.params.id) {
        return this.$route.params.id
      } else if (this.isSuperUser()) {
        return this.getOrgId;
      }
      return false
    }
  },
  async created() {
    if (isAdmin() || isSuperUser()) {
      this.clearUsers()
      await this.getUsers({ orgId: this.orgId })
    }
  },
  methods: {
    isAdmin,
    isSuperUser,
    ...mapActions('orgUsers', [
      'getUsers',
      'clearUsers'
    ]),
    handleEdit({ id }) {
      this.editUserId = id;
      this.$bvModal.show('modal-teammember')
    },
    handleRemove({ id }) {
      this.editUserId = id;
      this.$bvModal.show('modal-remove-teammember')
    },
    handleCreate() {
      this.$bvModal.show('modal-new-teammember')
    }
  }
}
</script>

<style scoped lang="scss">
h1 {
  font-size: 30px;
  color: #354052;
}

.panel {
  background: white;
  border-radius: 4px;
  border: 1px solid #E6EAEE;

  h2 {
    font-size: 22px;
    color: #354052;
  }
}
</style> 
