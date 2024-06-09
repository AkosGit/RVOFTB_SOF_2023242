import type { TagModel } from '@/models/TagModel'
import { BaseClient } from './BaseClient'

export class TagClient extends BaseClient {
  public async AddNewTag(tagName: string, snippetID: string): Promise<TagModel> {
    try {
      const resp = await this.GetAxios().post(this.baseUrl + '/tag/AddTagToSnippet', {
        snippetID: snippetID,
        tagName: tagName
      })
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
}
