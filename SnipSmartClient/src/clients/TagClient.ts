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
  public async RemoveTagsFromSnippet(snippetID: string) {
    try {
      const resp = await this.GetAxios().delete(
        this.baseUrl + '/tag/DeleteTagsFromSnippet/' + snippetID,
        {}
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetAllTags(): Promise<Array<TagModel>> {
    try {
      const resp = await this.GetAxios().get(this.baseUrl + '/tag', {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetDistinctNames(): Promise<Array<string>> {
    try {
      const resp = await this.GetAxios().get(this.baseUrl + '/tag/GetDistinctNames', {})
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
  public async GetTagsFromSnippet(snippetID: string): Promise<Array<TagModel>> {
    try {
      const resp = await this.GetAxios().get(
        this.baseUrl + '/tag/GetTagsFromSnippet/' + snippetID,
        {}
      )
      return resp.data
    } catch (error: any) {
      if (error.response) {
        throw new Error(`Failed to fetch data: "${error.response.data}"`)
      }
      throw new Error(`Failed to fetch data: ${error}`)
    }
  }
}
